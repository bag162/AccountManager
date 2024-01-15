using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.AspNetCore.Server.IIS.Core;
using Newtonsoft.Json;

namespace BASAccountManager.BackgroundTask
{
    public class StatusMonitor
    {
        private ILogger<StatusMonitor> logger;
        private ISMSServiceDB SMSServiceDB { get; set; }
        private IProxyDBService proxyDBService { get; set; }
        private ITaskDBService taskDbService { get; set; }
        private IWorkerTaskDBService workerTaskDbService { get; set; }
        private IPostCommentDBService postCommentDBService { get; set; }
        private IPostLikeDBService postLikeDBService { get; set; }
        private IFollowDBService followDBService { get; set; }

        public StatusMonitor(ITaskDBService taskDbService, 
            IWorkerTaskDBService workerTaskDbService, 
            IProxyDBService proxyDBService, 
            ISMSServiceDB SMSServiceDB, 
            ILogger<StatusMonitor> logger,
            IPostCommentDBService postCommentDBService,
            IPostLikeDBService postLikeDBService,
            IFollowDBService followDBService)
        {
            this.taskDbService = taskDbService;
            this.workerTaskDbService = workerTaskDbService;
            this.proxyDBService = proxyDBService;
            this.SMSServiceDB = SMSServiceDB;
            this.logger = logger;
            this.postCommentDBService = postCommentDBService;
            this.postLikeDBService = postLikeDBService;
            this.followDBService = followDBService;
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

                if(allWorkerTasks.Where(x => x.Proxy.Id == proxy.Id).Where(x => x.Status == DB.Models.TaskStatus.AtWork).Count() != 0)
                    continue;
                
                if (allWorkerTasks.Where(x => x.Proxy.Id == proxy.Id).Where(x => x.Status == DB.Models.TaskStatus.NotTaken).Count() != 0)
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
            var allPostComments = this.postCommentDBService.GetAllPostComments()
                .Where(x => x.CommentStatus == DB.Models.Post.CommentStatus.NotPublished)
                .Where(x => x.SenderAccountId != null).ToList();

            var allWorkerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();
            allWorkerTasks = allWorkerTasks
                .Where(x => x.TaskType == DB.Models.TaskType.Commenting)
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken)
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
                    postComment.SenderAccountId = null;
                    listToUpdate.Add(postComment);
                }
            }

            await this.postCommentDBService.UpdatePostCommentAsync(listToUpdate);

            var commentsToDelete = allPostComments.Where(x => x.CommentStatus == CommentStatus.ErrorPublication).ToList();
            await this.postCommentDBService.RemoveCommentsAsync(commentsToDelete);
            return;
        }

        public async Task CheckUntakenLikes()
        {
            var allPostLikes = this.postLikeDBService.GetAllLikes()
                .Where(x => x.LikeStatus == LikeStatus.NotPublished)
                .Where(x => x.SenderAccountId != null).ToList();

            var allWorkerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();
            allWorkerTasks = allWorkerTasks
                .Where(x => x.TaskType == DB.Models.TaskType.Liking)
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken)
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
                    postLike.SenderAccountId = null;
                    listToUpdate.Add(postLike);
                }
            }

            await this.postLikeDBService.UpdateLikesAsync(listToUpdate);

            var likesToRemove = allPostLikes.Where(x => x.LikeStatus == LikeStatus.ErrorPublication).ToList();
            await this.postLikeDBService.RemoveLikesAsync(likesToRemove);
            return;
        }

        public async Task CheckUntakenFollows()
        {
            var allWorkerTasks = await this.workerTaskDbService.GetWorkerTasksAsync();

            var activeAccounts = allWorkerTasks
                .Where(x => x.TaskType == DB.Models.TaskType.Following)
                .Where(x => x.Status == DB.Models.TaskStatus.NotTaken)
                .Select(x => x.Account)
                .ToList();

            var follows = this.followDBService.GetFollows().ToList();
            var followsToCheck = follows
                .Where(x => x.FollowStatus == DB.Models.FollowStatus.NotPublished)
                .ToDictionary(x => x.SenderAccountId);

            foreach (var account in activeAccounts)
            {
                if (followsToCheck.ContainsKey(account.Id))
                {
                    followsToCheck.Remove(account.Id);
                }
            }

            await this.followDBService.RemoveByIdsAsync(followsToCheck.Select(x => x.Value).Select(x => x.Id).ToList());

            var errorFollows = follows.Where(x => x.FollowStatus == DB.Models.FollowStatus.ErrorFollow).ToList();
            await this.followDBService.RemoveFollows(errorFollows);
        }
    }
}