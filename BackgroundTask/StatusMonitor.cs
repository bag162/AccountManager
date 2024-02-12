using AutoMapper.Internal.Mappers;
using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.AdvertResourses;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.AspNetCore.Server.IIS.Core;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

namespace BASAccountManager.BackgroundTask
{
    public class StatusMonitor
    {
        private ISMSServiceDB SMSServiceDB { get; set; }
        private IProxyDBService proxyDBService { get; set; }
        private ITaskDBService taskDbService { get; set; }
        private IWorkerTaskDBService workerTaskDbService { get; set; }
        private IPostCommentDBService postCommentDBService { get; set; }
        private IPostLikeDBService postLikeDBService { get; set; }
        private IFollowDBService followDBService { get; set; }
        private IAdvertAccountDBService advertAccountDBService { get; set; }
        private IAdvertPostDBService advertPostDBService { get; set; }
        private IClonDBService clonDBService { get; set; }
        private IPostCommentGroupDBService postCommentGroupDBService { get; set; }
        private IPostGroupDBService postGroupDBService { get; set; }
        private IFillingDataDBService fillingDataDBService { get; set; }

        public StatusMonitor(ITaskDBService taskDbService, 
            IWorkerTaskDBService workerTaskDbService, 
            IProxyDBService proxyDBService, 
            ISMSServiceDB SMSServiceDB,
            IPostCommentDBService postCommentDBService,
            IPostLikeDBService postLikeDBService,
            IFollowDBService followDBService,
            IAdvertAccountDBService advertAccountDBService,
            IAdvertPostDBService advertPostDBService,
            IClonDBService clonDBService,
            IPostCommentGroupDBService postCommentGroupDBService,
            IPostGroupDBService postGroupDBService,
            IFillingDataDBService fillingDataDBService)
        {
            this.taskDbService = taskDbService;
            this.workerTaskDbService = workerTaskDbService;
            this.proxyDBService = proxyDBService;
            this.SMSServiceDB = SMSServiceDB;
            this.postCommentDBService = postCommentDBService;
            this.postLikeDBService = postLikeDBService;
            this.followDBService = followDBService;
            this.advertPostDBService = advertPostDBService;
            this.advertAccountDBService = advertAccountDBService;
            this.clonDBService = clonDBService;
            this.postCommentGroupDBService = postCommentGroupDBService;
            this.postGroupDBService = postGroupDBService;
            this.fillingDataDBService = fillingDataDBService;
        }

        public async Task CheckTaskWorkerStatusAsync()
        {
            var tasks = this.taskDbService.GetTask();
            foreach (var task in tasks)
            {
                if (task.Status != DB.Models.StatusTask.Performed)
                {
                    continue;
                }

                var workerTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);

                if (workerTasks.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).Count() != 0)
                    continue;
                if (workerTasks.Where(x => x.Status == DB.Models.TaskStatus.AtWork).Count() != 0)
                    continue;

                task.Status = DB.Models.StatusTask.Completed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
        }

        public async Task CheckProxyStatusAsync()
        {
            var allWorkerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();
            var proxyList = this.proxyDBService.GetProxy();
            foreach (var proxy in proxyList)
            {
                if (proxy.ProxyStatus == DB.Models.ProxyStatus.Free)
                {
                    continue;
                }

                if(allWorkerTasks.Where(x => x.Proxy.Id == proxy.Id).Where(x => x.Status == DB.Models.TaskStatus.AtWork || x.Status == DB.Models.TaskStatus.NotTaken).Count() != 0)
                    continue;
                
                await this.proxyDBService.SetProxyFreeStatusAsync(proxy.Id);
            }
        }

        public async Task CheckInactiveTask()
        {
            var tasks = this.taskDbService.GetTask().Where(x => x.Status == DB.Models.StatusTask.Canceled).ToList();
            var completedTasks = this.taskDbService.GetTask().Where(x => x.Status == DB.Models.StatusTask.Completed).ToList();
            tasks.AddRange(completedTasks);

            foreach (var task in tasks)
            {
                var workerTasks = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);
                var noTakenTask = workerTasks.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).ToList();
                if (noTakenTask.Count() != 0)
                {
                    await this.workerTaskDbService.RemoveWorkerTaskAsync(noTakenTask);
                }
            }
        }

        public async Task CheckUntakenComments()
        {
            var allComments = this.postCommentDBService.GetAllPostComments();
            var allPostComments = allComments
                .Where(x => x.CommentStatus == DB.Models.Post.CommentStatus.NotPublished)
                .Where(x => x.SenderAccountId != null).ToList();

            var allWorkerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();
            allWorkerTasks = allWorkerTasks
                .Where(x => x.TaskType == DB.Models.TaskType.Commenting)
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken || x.Status == DB.Models.TaskStatus.AtWork)
                .ToList();

            var activeCommentsId = new List<int>();

            foreach (var worker in allWorkerTasks)
            {
                var commentDTO = JsonConvert.DeserializeObject<List<CommentUsefulDataDTO>>(worker.UsefulData);
                activeCommentsId.AddRange(commentDTO.Select(x => x.PostCommentId).ToList());
            }

            var listToUpdate = new List<DBPostComment>();

            foreach (var postComment in allPostComments)
            {
                if (!activeCommentsId.Contains(postComment.Id))
                {
                    if (DateTime.Now - postComment.CommentTime >= TimeSpan.FromMinutes(30))
                    {
                        postComment.SenderAccountId = null;
                        listToUpdate.Add(postComment);
                    }
                }
            }

            await this.postCommentDBService.UpdatePostCommentAsync(listToUpdate);

            var commentsToDelete = allPostComments.Where(x => x.CommentStatus == CommentStatus.ErrorPublication).ToList();
            await this.postCommentDBService.RemoveCommentsAsync(commentsToDelete);

            var inProcessComments = allComments.Where(x => x.CommentStatus == CommentStatus.InProcessPublication).ToList();
            var CommentsToDelete = new List<DBPostComment>();
            foreach (var item in inProcessComments)
            {
                if (DateTime.Now - item.CommentTime >= TimeSpan.FromMinutes(30))
                {
                    CommentsToDelete.Add(item);
                }
            }
            await this.postCommentDBService.RemoveCommentsAsync(CommentsToDelete);
        }

        public async Task CheckUntakenLikes()
        {
            var allLikes = this.postLikeDBService.GetAllLikes();
            var allPostLikes = allLikes
                .Where(x => x.LikeStatus == LikeStatus.NotPublished)
                .Where(x => x.SenderAccountId != null).ToList();

            var allWorkerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();
            allWorkerTasks = allWorkerTasks
                .Where(x => x.TaskType == DB.Models.TaskType.Liking)
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken || x.Status == DB.Models.TaskStatus.AtWork)
                .ToList();

            var activeLikesId = new List<int>();

            foreach (var worker in allWorkerTasks)
            {
                var likeDTO = JsonConvert.DeserializeObject<List<LikeUsefulDataDTO>>(worker.UsefulData);
                activeLikesId.AddRange(likeDTO.Select(x => x.PostLikeId).ToList());
            }

            var listToUpdate = new List<DBPostLikes>();

            foreach (var postLike in allPostLikes)
            {
                if (!activeLikesId.Contains(postLike.Id))
                {
                    if (DateTime.Now - postLike.CreatedDate >= TimeSpan.FromMinutes(30))
                    {
                        postLike.SenderAccountId = null;
                        listToUpdate.Add(postLike);
                    }
                }
            }

            await this.postLikeDBService.UpdateLikesAsync(listToUpdate);

            var likesToRemove = allPostLikes.Where(x => x.LikeStatus == LikeStatus.ErrorPublication).ToList();
            await this.postLikeDBService.RemoveLikesAsync(likesToRemove);

            var inProcessLikes = allLikes.Where(x => x.LikeStatus == LikeStatus.InProcessPublication).ToList();
            var LikesToDelete = new List<DBPostLikes>();
            foreach (var item in inProcessLikes)
            {
                if (DateTime.Now - item.CreatedDate >= TimeSpan.FromMinutes(30))
                {
                    LikesToDelete.Add(item);
                }
            }
            await this.postLikeDBService.RemoveLikesAsync(LikesToDelete);
        }

        public async Task CheckUntakenFollows()
        {
            var allWorkerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();

            var activeAccounts = allWorkerTasks
                .Where(x => x.TaskType == DB.Models.TaskType.Following)
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken || x.Status == DB.Models.TaskStatus.AtWork)
                .Select(x => x.Account)
                .ToList();

            var follows = this.followDBService.GetFollows().ToList();
            var followsToCheck = follows
                .Where(x => x.FollowStatus == DB.Models.FollowStatus.NotPublished)
                .ToList();

            List<DBFollow> listToDelete = new();
            foreach (var account in activeAccounts)
            {
                
                if (followsToCheck.Where(x => x.SenderAccountId == account.Id).Count() != 0)
                {
                    var deletedFollows = followsToCheck.Where(x => x.SenderAccountId == account.Id).ToList();
                    foreach (var delFollow in deletedFollows)
                    {
                        if (DateTime.Now - delFollow.CreatedDate >= TimeSpan.FromMinutes(30))
                        {
                            listToDelete.Add(delFollow);
                        }
                        
                    }
                }
            }

            await this.followDBService.RemoveByIdsAsync(listToDelete.Select(x => x.Id).ToList());

            var errorFollows = follows.Where(x => x.FollowStatus == DB.Models.FollowStatus.ErrorFollow).ToList();
            await this.followDBService.RemoveFollows(errorFollows);

            // Delete process follows
            var inProcessFollows = follows.Where(x => x.FollowStatus == FollowStatus.InProcessFollow).ToList();
            var followsToDelete = new List<DBFollow>();
            foreach (var item in inProcessFollows)
            {
                if (DateTime.Now - item.CreatedDate >= TimeSpan.FromMinutes(30))
                {
                    followsToDelete.Add(item);
                }
            }
            await this.followDBService.RemoveByIdsAsync(inProcessFollows.Select(x => x.Id).ToList());
        }

        public async Task CheckAdvertUntakenFollows()
        {
            // Получаем аккаунты в процессе выполнения
            var advertAccounts = this.advertAccountDBService.GetAdvertAccounts()
                .Where(x => x.AdvertAccountStatus == AdvertAccountStatus.ProcessTreatment)
                .ToList();

            // Получаем невзятые задачи или задачи в процессе выполнения
            var taskWorkers = await this.workerTaskDbService.GetWorkerTasksAsync();
            taskWorkers = taskWorkers
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken || x.Status == DB.Models.TaskStatus.AtWork)
                .Where(x => x.TaskType == DB.Models.TaskType.AdvertFollowing)
                .ToList();

            foreach (var worker in taskWorkers)
            {
                // Вытягиваем из воркера аккаунты
                var advertAccs = JsonConvert.DeserializeObject<List<DBAdvertAccount>>(worker.UsefulData);
                foreach (var advertAccount in advertAccs)
                {
                    // Если в списке рекламных аккаунтов есть аккаунт, который находится в воркере, то удаляем его
                    if (advertAccounts.Where(x => x.Id == advertAccount.Id).Count() != 0)
                    {
                        advertAccounts.Remove(advertAccounts.Where(x => x.Id == advertAccount.Id).First());
                    }
                }
            }
            // Обновляем рекламные аккаунты, которых нет в невзятых или находящихся в процессе выполнения воркеров
            foreach (var advertAccount in advertAccounts)
            {
                advertAccount.AdvertAccountStatus = AdvertAccountStatus.NotProcessed;
            }
            await this.advertAccountDBService.UpdateAvertAccountsAsync(advertAccounts);
        }

        public async Task CheckAdvertUntakenLikesAndComments()
        {
            var advertPosts = this.advertPostDBService.GetAdvertPosts()
                .Where(x => x.AdvertPostLikeStatus == AdvertPostActionStatus.ProcessTreatment || x.AdvertPostCommentStatus == AdvertPostActionStatus.ProcessTreatment)
                .ToList();

            var taskWorkers = await this.workerTaskDbService.GetWorkerTasksAsync();
            taskWorkers = taskWorkers
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken || x.Status == DB.Models.TaskStatus.AtWork)
                .Where(x => x.TaskType == DB.Models.TaskType.AdvertCommenting || x.TaskType == DB.Models.TaskType.AdvertLiking)
                .ToList();

            // liking check
            var likingPosts = advertPosts.Where(x => x.AdvertPostLikeStatus == AdvertPostActionStatus.ProcessTreatment).ToList();
            var likingWorkers = taskWorkers.Where(x => x.TaskType == DB.Models.TaskType.AdvertLiking).ToList();

            foreach (var worker in likingWorkers)
            {
                var advertLikePosts = JsonConvert.DeserializeObject<List<DBAdvertPost>>(worker.UsefulData);
                foreach (var advertLikePost in advertLikePosts)
                {
                    if (likingPosts.Where(x => x.Id == advertLikePost.Id).Count() != 0)
                    {
                        likingPosts.Remove(likingPosts.Where(x => x.Id == advertLikePost.Id).First());
                    }
                }
            }

            foreach (var updatedLikePost in likingPosts)
            {
                updatedLikePost.AdvertPostLikeStatus = AdvertPostActionStatus.NotProcessed;
            }
            await this.advertPostDBService.UpdateAdvertPostAsync(likingPosts);

            // commenting check

            var commentingPosts = advertPosts.Where(x => x.AdvertPostCommentStatus == AdvertPostActionStatus.ProcessTreatment).ToList();
            var commentingWorkers = taskWorkers.Where(x => x.TaskType == DB.Models.TaskType.AdvertCommenting).ToList();

            foreach (var worker in commentingWorkers)
            {
                var advertCommentingPosts = JsonConvert.DeserializeObject<List<DBAdvertPost>>(worker.UsefulData);
                foreach (var advertCommentPost in advertCommentingPosts)
                {
                    if (commentingPosts.Where(x => x.Id == advertCommentPost.Id).Count() != 0)
                    {
                        commentingPosts.Remove(commentingPosts.Where(x => x.Id == advertCommentPost.Id).First());
                    }
                }
            }

            foreach (var updatedCommentPost in commentingPosts)
            {
                updatedCommentPost.AdvertPostCommentStatus = AdvertPostActionStatus.NotProcessed;
            }

            await this.advertPostDBService.UpdateAdvertPostAsync(commentingPosts);
        }

        public async Task CheckUntakenClon()
        {
            var allClones = this.clonDBService.GetClones();

            var deletedClones = allClones
                .Where(x => x.ClonStatus == ClonStatus.NotProcessed)
                .Where(x => DateTime.Now - x.CreateTime >= TimeSpan.FromHours(1))
                .ToList();

            await this.clonDBService.DeleteClonesAsync(deletedClones);
        }

        public async Task CheckWorkerTaskError()
        {
            var workerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();
            workerTasks = workerTasks.Where(x => x.Status == DB.Models.TaskStatus.AtWork).ToList();
            List<DBWorkerTask> taskToDelete = new();

            foreach (var workerTask in workerTasks)
            {
                switch (workerTask.TaskType)
                {
                    case TaskType.ParseCloningInformation:
                        if (DateTime.Now - workerTask.CreatedDate >= TimeSpan.FromMinutes(60))
                        {
                            taskToDelete.Add(workerTask);
                        }
                        break;
                    default:
                        if (DateTime.Now - workerTask.CreatedDate >= TimeSpan.FromMinutes(10))
                        {
                            taskToDelete.Add(workerTask);
                        }
                        break;
                }
            }

            await workerTaskDbService.RemoveWorkerTaskAsync(taskToDelete);
        }
    }
}