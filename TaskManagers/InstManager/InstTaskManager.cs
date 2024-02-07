using AutoMapper;
using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.Controllers.Clon.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.Controllers.ProfileFilling.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.AdvertResourses;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using BASAccountManager.TaskManagers.InstManager.DTO;
using Hangfire.Server;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BASAccountManager.TaskManagers.InstManager
{
    public class InstTaskManager
    {
        private IMapper mapper;
        private ILogger<InstTaskManager> Logger { get; set; }
        private IWorkerTaskDBService workerTaskDBService { get; set; }
        private ITaskDBService taskDBService { get; set; }
        private IInstDBService instDBService { get; set; }
        private IBASExeptionDBService BASExeption { get; set; }
        private IInstPostDBService instPostDBService { get; set; }
        private IPostCommentDBService postCommentDBService { get; set; }
        private IPostLikeDBService postLikeDBService { get; set; }
        private IFollowDBService followDBService { get; set; }
        private IProxyDBService proxyDBService { get; set; }
        private IAdvertPostDBService advertPostDBService { get; set; }
        private IAdvertAccountDBService advertAccountDBService { get; set; }
        private IClonDBService clonDBService { get; set; }
        private IClonGroupDBService clonGroupDBService { get; set; }
        private IFillingDataDBService fillingDataDBService { get; set; }
        private IPostGroupDBService postGroupDBService { get; set; }
        private IPostDBService postDBService { get; set; }
        private ICommentDBService commentDBService { get; set; }
        private IPostCommentGroupDBService postCommentGroupDBService { get; set; }

        public InstTaskManager(ILogger<InstTaskManager> logger,
            IMapper mapper,
            IWorkerTaskDBService workerTaskDBService,
            IInstDBService instDBService,
            ITaskDBService taskDBService,
            IBASExeptionDBService BASExeption,
            IInstPostDBService instPostDBService,
            IPostCommentDBService postCommentDBService,
            IPostLikeDBService postLikeDBService,
            IFollowDBService followDBService,
            IProxyDBService proxyDBService,
            IAdvertAccountDBService advertAccountDBService,
            IAdvertPostDBService advertPostDBService,
            IClonDBService clonDBService,
            IClonGroupDBService clonGroupDBService,
            IFillingDataDBService fillingDataDBService,
            IPostGroupDBService postGroupDBService,
            IPostDBService postDBService,
            ICommentDBService commentDBService,
            IPostCommentGroupDBService postCommentGroupDBService)
        {
            this.Logger = logger;
            this.mapper = mapper;
            this.workerTaskDBService = workerTaskDBService;
            this.instDBService = instDBService;
            this.taskDBService = taskDBService;
            this.BASExeption = BASExeption;
            this.instPostDBService = instPostDBService;
            this.postCommentDBService = postCommentDBService;
            this.postLikeDBService = postLikeDBService;
            this.followDBService = followDBService;
            this.proxyDBService = proxyDBService;
            this.advertAccountDBService = advertAccountDBService;
            this.advertPostDBService = advertPostDBService;
            this.clonDBService = clonDBService;
            this.clonGroupDBService = clonGroupDBService;
            this.fillingDataDBService = fillingDataDBService;
            this.postGroupDBService = postGroupDBService;
            this.postDBService = postDBService;
            this.commentDBService = commentDBService;
            this.postCommentGroupDBService = postCommentGroupDBService;
        }

        public async Task<string> GetTaskAsync(GetTaskDTO getTaskData)
        {
            var allTasks = await this.workerTaskDBService.GetWorkerTasksAsync();

            if (allTasks.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).Count() == 0)
                return "No tasks";

            var returnedTask = new DBWorkerTask();

            // Фильтруем задачи, что бы 1 аккаунт не выполнял одновременно 2-е задачи
            foreach (var task in allTasks.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).OrderBy(x => Guid.NewGuid()))
            {
                if (allTasks.Where(x => x.AccountId == task.AccountId).Where(x => x.Status == DB.Models.TaskStatus.AtWork).Count() == 0)
                {
                    returnedTask = task;
                }
            }

            if (returnedTask.Task == null)
            {
                return "No tasks";
            }

            switch (returnedTask.TaskType)
            {
                case TaskType.RegistrationAccounts:
                    return await RegistrationTaskImplAsync(returnedTask, getTaskData);
                case TaskType.AuthorizationAccounts:
                    return await AuthorizationTaskImplAsync(returnedTask, getTaskData);
                case TaskType.Posting:
                    return await PostingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.Commenting:
                    return await CommentingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.Liking:
                    return await LikingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.Following:
                    return await FollowingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.FillingProfile:
                    return await ProfileFillingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.AdvertFollowing:
                    return await AdvertFollowingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.AdvertCommenting:
                    return await AdvertCommentingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.AdvertLiking:
                    return await AdvertLikingTaskImplAsync(returnedTask, getTaskData);
                case TaskType.ParseCloningInformation:
                    return await ParseCloningInformation(returnedTask, getTaskData);

            }
            this.Logger.LogWarning("Skip switch on InstTaskManager");
            return "No tasks";
        }


        public async Task<string> EndRegistrationTaskAsync(EndRegistrationTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.TaskWorkerId);
            DBInstagramAccount newAccount = this.mapper.Map<DBInstagramAccount>(endData);
            newAccount.InstanceId = taskWorker.InstanceId;
            newAccount.AccountStatus = AccountStatus.Authorized;
            var accountId = await this.instDBService.AddInstAccountsAsync(newAccount, taskWorker.Task.AccountGroup);

            taskWorker.AccountId = accountId;
            taskWorker.Status = DB.Models.TaskStatus.Completed;

            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndAuthorizationTaskAsync(EndAuthorizationTaskDTO endData)
        {
            if (endData.PhoneNumber == "")
                endData.PhoneNumber = null;
            if (endData.Email == "")
                endData.Email = null;

            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.TaskWorkerId);
            taskWorker.Account.ProfileLink = endData.ProfileLink;
            taskWorker.Account.Name = endData.Name;
            taskWorker.Account.Surname = endData.Surname;
            taskWorker.Account.Email = endData.Email;
            taskWorker.Account.PhoneNumber = endData.PhoneNumber;
            taskWorker.Account.AccountStatus = AccountStatus.Authorized;
            taskWorker.Account.InstanceId = taskWorker.InstanceId;
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);

        }

        public async Task<string> EndPostingTaskAsync(EndPostingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.workerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndPostingTaskAsync(IntermediateEndPostingTaskDTO endData)
        {
            var worker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.workerId);
            var instpost = this.instPostDBService.GetInstPospsByAccount(worker.AccountId).Where(x => x.PostId == endData.postId).First();
            instpost.CreatedDate = DateTime.Now;
            instpost.PostURI = endData.postURI;
            instpost.InstPostStatus = DB.Models.Post.InstPostStatus.Published;
            await this.instPostDBService.UpdateInstPostAsync(instpost);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndCommentingTaskAsync(EndCommentingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndCommentingTask(IntermediateEndCommentingTaskDTO endData)
        {
            var postComment = this.postCommentDBService.GetPostCommentById(endData.PostCommentId);
            postComment.CommentTime = DateTime.Now;
            postComment.CommentStatus = DB.Models.Post.CommentStatus.Published;
            await this.postCommentDBService.UpdatePostCommentAsync(postComment);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndLikingTaskAsync(EndLikingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndLikingTask(IntermediateEndLikingTaskDTO endData)
        {
            var postLike = this.postLikeDBService.GetPostLikeById(endData.PostLikeId);
            postLike.CreatedDate = DateTime.Now;
            postLike.LikeStatus = LikeStatus.Published;
            await this.postLikeDBService.UpdateLikeAsync(postLike);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndFollowingTaskTasync(EndFollowingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndFollowingTask(IntermediateEndFollowingTaskDTO endData)
        {
            var follow = this.followDBService.GetFollowById(endData.FollowId);
            follow.CreatedDate = DateTime.Now;
            follow.FollowStatus = FollowStatus.Published;
            await this.followDBService.UpdateFollowAsync(follow);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndProfileFillingTaskAsync(EndProfileFillingTask endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            var profileFillingData = JsonConvert.DeserializeObject<ProfileFillingUsefulDataDTO>(taskWorker.UsefulData);

            taskWorker.Status = DB.Models.TaskStatus.Completed;
            taskWorker.Account.FillingDataId = profileFillingData.Id;

            if (endData.NewName != "not")
            {
                taskWorker.Account.Name = endData.NewName;
                taskWorker.Account.Surname = endData.NewSurname;
            }
            if (endData.NewSurname != "not")
            {
                taskWorker.Account.AccountStatus = AccountStatus.NotAuthorized;
                taskWorker.Account.Login = endData.NewUsername;
                taskWorker.Account.ProfileLink = "instagram.com/" + endData.NewUsername;
            }
            

            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndAdvertFollowingTask(EndIntermediateAdvertFollowingTaskDTO endData)
        {
            var advertAccount = this.advertAccountDBService.GetAdvertAccountById(endData.AdvertAccountId);
            advertAccount.AdvertAccountStatus = AdvertAccountStatus.Processed;
            await this.advertAccountDBService.UpdateAvertAccountsAsync(new List<DBAdvertAccount>() { advertAccount });
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndAdvertFollowingTask(EndAdvertFollowingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndAdvertLikingTask(EndIntermediateAdvertLikingTaskDTO endData)
        {
            var advertPost = this.advertPostDBService.GetAdvertPostById(endData.AdvertPostId);
            advertPost.AdvertPostLikeStatus = AdvertPostActionStatus.Processed;
            await this.advertPostDBService.UpdateAdvertPostAsync(new List<DBAdvertPost>() { advertPost });
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndAdvertLikingTask(EndAdvertLikingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndAdvertCommentingTask(EndIntermediateAdvertCommentingTaskDTO endData)
        {
            var advertPost = this.advertPostDBService.GetAdvertPostById(endData.AdvertPostId);
            advertPost.AdvertPostCommentStatus = AdvertPostActionStatus.Processed;
            await this.advertPostDBService.UpdateAdvertPostAsync(new List<DBAdvertPost>() { advertPost });
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndAdvertCommentingTask(EndAdvertCommentingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndParseCloningInformationProfileData(EndIntermediateParseCloningInformationProfileData endData)
        {
            var clon = this.clonDBService.GetClonById(endData.ClonId);
            var fillingData = new AddFillingDataDTO()
            {
                AboutMe = endData.ProfileDescription,
                ClosedAccount = false,
                EnableRecomendations = true,
                Gender = "Male",
                NameOrSurnameGenString = "<RMaleName>:<RSurname>",
                UsernameGenString = "{<ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><AnyDigit><AnyDigit>|<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow>_<ELowVow><ELowCons><ELowVow><ELowCons>|<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow>_<ELowVow><ELowCons><ELowVow><ELowCons><AnyDigit><AnyDigit><AnyDigit><AnyDigit>|<EFemNameLow>_<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow>|<EFemNameLow>_<ELowCons><ELowVow><ELowCons><ELowVow><ELowCons><ELowVow><AnyDigit><AnyDigit><AnyDigit><AnyDigit>}",
                Name = "z_clon_" + clon.Id,
                AvatarBASE64 = endData.ImageBase64Data,
                AvatarFormat = endData.ImageFormat
            };
            var fillingDataId = await this.fillingDataDBService.AddFillingDataAsync(fillingData);
            clon.FillingDataId = fillingDataId;
            await this.clonDBService.UpdateCloneAsync(clon);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndParseCloningInformationPostData(EndIntermediateParseCloningInformationPostData endData)
        {
            var clon = this.clonDBService.GetClonById(endData.ClonId);
            int clonPostId;
            if (clon.PostGroupId == null)
            {
                var postGroupId = await this.postGroupDBService.AddGroupAsync(new DBPostGroup() { Name = "z_clon_" + clon.Id, PostGroupType = PostGroupType.Clon });
                clon.PostGroupId = postGroupId;
                await this.clonDBService.UpdateCloneAsync(clon);
            }

            await this.postCommentGroupDBService.AddPostCommentGroupAsync(new DBPostCommentGroup() { Name = "z_clon_" + clon.Id + "_" + endData.PostURI});
            var comments = new List<CRUDCommentDTO>();
            foreach (var item in endData.Comments)
            {
                comments.Add(new CRUDCommentDTO() { CommentGroupName = "z_clon_" + clon.Id + "_" + endData.PostURI, Message = item });
            }
            await this.commentDBService.AddCommentAsync(comments);

            await this.postDBService.AddPostAsync(new CRUDPostDTO()
            {
                Description = endData.Description,
                Name = "z_clon_" + clon.Id + "_" + Guid.NewGuid(),
                ImageBase64 = endData.ImageBase64Data,
                ImageFormat = endData.ImageFormat,
                PostGroupName = "z_clon_" + clon.Id,
                PostStatus = "Active",
                RequiredCountComments = Random.Shared.Next(0, 10),
                RequiredCountLikes = Random.Shared.Next(10, 40),
                PostURI = endData.PostURI,
                CommentGroupName = "z_clon_" + clon.Id + "_" + endData.PostURI
            });
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> EndParseCloningInformationData(EndIntermediateParseCloningInformationData endData)
        {
            var clon = this.clonDBService.GetClonById(endData.ClonId);
            clon.ClonStatus = ClonStatus.Processed;
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.WorkerId);
            taskWorker.Status = DB.Models.TaskStatus.Completed;

            await this.proxyDBService.SetProxyFreeStatusAsync(taskWorker.Proxy.Id);
            await this.clonDBService.UpdateCloneAsync(clon);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }


        public async Task<string> ErrorRegistrationTaskAsync(RegistrationTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();
            // Изменяем статус головной задачи на "В процессе добавления", т.к. необходимо восполнить неудачно завершившуюся задачу новой.
            workerTask.Task.Status = StatusTask.AddingProcess;
            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorAuthorizationTaskAsync(AuthorizationTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();
            switch (error)
            {
                case AuthorizationTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case AuthorizationTaskErrorType.IncorrectAuthData:
                    workerTask.Account.AccountStatus = AccountStatus.IncorrectCredentionalData;
                    break;
            }
            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorVerifyServiceAsync(VerifyServiceErrorDTO error)
        {
            switch (error.VerifyServiceTypeError)
            {
                case VerifyServiceErrorType.LowBalance:
                    var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(error.workerId);
                    workerTask.Status = DB.Models.TaskStatus.Error;
                    workerTask.ErrorMessage = "Low balance VerifyService";
                    workerTask.Task.Status = StatusTask.Canceled;
                    await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
                    await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
                    break;
            }

            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorPostingTaskAsync(PostingTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();
            

            switch (error)
            {
                case PostingTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case PostingTaskErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorIntermediatePostingAsync(int workerId, int postID, string errorMessage)
        {
            var worker = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            var instpost = this.instPostDBService.GetInstPospsByAccount(worker.AccountId).Where(x => x.PostId == postID).First();
            instpost.InstPostStatus = DB.Models.Post.InstPostStatus.PostingError;
            instpost.PostingErrorMessage = errorMessage;
            await this.instPostDBService.UpdateInstPostAsync(instpost);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorCommentingTaskAsync(CommentingTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case CommentingTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case CommentingTaskErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorIntermediateCommentingAsync(int WorkerId, int PostCommentId, string ErrorMessage)
        {
            var postComment = this.postCommentDBService.GetPostCommentById(PostCommentId);
            postComment.CommentStatus = DB.Models.Post.CommentStatus.ErrorPublication;
            postComment.ErrorMessage = ErrorMessage;
            await this.postCommentDBService.UpdatePostCommentAsync(postComment);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorLikingTaskAsync(LikingTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case LikingTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case LikingTaskErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorIntermediateLikingAsync(int WorkerId, int PostLikeId, string ErrorMessage)
        {
            var postLike = this.postLikeDBService.GetPostLikeById(PostLikeId);
            postLike.LikeStatus = LikeStatus.ErrorPublication;
            postLike.ErrorMessage = ErrorMessage;
            await this.postLikeDBService.UpdateLikeAsync(postLike);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorFollowingTaskAsync(FollowingTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case FollowingTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case FollowingTaskErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorIntermediateFollowingAsync(int WorkerId, int followId, string ErrorMessage)
        {
            var follow = this.followDBService.GetFollowById(followId);
            follow.FollowStatus = FollowStatus.ErrorFollow;
            follow.ErrorMessage = ErrorMessage;
            await this.followDBService.UpdateFollowAsync(follow);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorProfileFillingTaskAsync(ProfileFillingTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case ProfileFillingTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case ProfileFillingTaskErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorIntermediateAdvertFollowingAsync(int WorkerId, int advertAccountId, string ErrorMessage)
        {
            var advertAccount = this.advertAccountDBService.GetAdvertAccountById(advertAccountId);
            advertAccount.AdvertAccountStatus = AdvertAccountStatus.Error;
            advertAccount.ErrorMessage = ErrorMessage;
            await this.advertAccountDBService.UpdateAvertAccountsAsync(new List<DBAdvertAccount>() { advertAccount });
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorAdvertFollowingAsync(AdvertFollowingErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case AdvertFollowingErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case AdvertFollowingErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorIntermediateAdvertLikingAsync(int WorkerId, int advertPostId, string ErrorMessage)
        {
            var advertPost = this.advertPostDBService.GetAdvertPostById(advertPostId);
            advertPost.AdvertPostLikeStatus = AdvertPostActionStatus.Error;
            advertPost.LikingErrorMessage = ErrorMessage;
            await this.advertPostDBService.UpdateAdvertPostAsync(new List<DBAdvertPost>() { advertPost });
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorAdvertLikingAsync(AdvertLikingErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case AdvertLikingErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case AdvertLikingErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorIntermediateAdvertCommentingAsync(int WorkerId, int advertPostId, string ErrorMessage)
        {
            var advertPost = this.advertPostDBService.GetAdvertPostById(advertPostId);
            advertPost.AdvertPostCommentStatus = AdvertPostActionStatus.Error;
            advertPost.CommentingErrorMessage = ErrorMessage;
            await this.advertPostDBService.UpdateAdvertPostAsync(new List<DBAdvertPost>() { advertPost });
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorAdvertCommentingAsync(AdvertCommentingErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case AdvertCommentingErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case AdvertCommentingErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorParseCloningInformation(int workerId, int ClonId, ParseCloningInformationErrorType error)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();

            switch (error)
            {
                case ParseCloningInformationErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case ParseCloningInformationErrorType.DeauthorizedError:
                    workerTask.Account.AccountStatus = AccountStatus.NotAuthorized;
                    break;
                case ParseCloningInformationErrorType.PageNotAvailable:
                    var clon = this.clonDBService.GetClonById(ClonId);
                    clon.ClonStatus = ClonStatus.NotAvailable;
                    await this.clonDBService.UpdateCloneAsync(clon);
                    break;
            }

            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorGlobalAsync(GlobalErrorDTO error)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(error.workerId);
            DBBASExeption exeption = new DBBASExeption
            {
                AccountId = workerTask.AccountId,
                TaskId = workerTask.TaskId,
                ProxyId = workerTask.ProxyId,
                ExeptionMessage = error.ExeptionMessage,
                ExeptionTime = DateTime.Now
            };

            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = "GlobalExeption: " + error.ExeptionMessage;
            await this.BASExeption.AddAsync(exeption);
            await this.proxyDBService.SetProxyFreeStatusAsync(workerTask.Proxy.Id);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }
        


        private async Task<string> RegistrationTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);


            switch (returnedTask.RegistrationVerifyResoursesType)
            {
                case RegistrationVerifyResoursesType.SMSService:
                    var smsTask = this.mapper.Map<GetRegistrationTaskSMSServiceDTO>(returnedTask);
                    smsTask.UsefulData = JsonConvert.DeserializeObject<RegistrationClientTaskWorkerUsefulDataSMSServiceDTO>(returnedTask.UsefulData);
                    return JsonConvert.SerializeObject(smsTask);

                case RegistrationVerifyResoursesType.EmailService:
                    var emailTask = this.mapper.Map<GetRegistrationTaskEmailServiceDTO>(returnedTask);
                    emailTask.UsefulData = JsonConvert.DeserializeObject<RegistrationClientTaskWorkerUsefulDataEmailServiceDTO>(returnedTask.UsefulData);
                    return JsonConvert.SerializeObject(emailTask);
            }
            this.Logger.LogWarning("Processing implementation not found for RegResourceType: " + returnedTask.RegistrationVerifyResoursesType);
            return null;
        }

        private async Task<string> AuthorizationTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;
            

            var task = this.mapper.Map<GetAuthorizationTaskDTO>(returnedTask);
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> PostingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            var postList = this.instPostDBService.GetInstPospsByAccount(returnedTask.AccountId);
            if (postList.Where(x => x.InstPostStatus == DB.Models.Post.InstPostStatus.NotPublished || x.InstPostStatus == InstPostStatus.PostingError).Count() == 0)
            {
                returnedTask.Status = DB.Models.TaskStatus.Completed;
                await this.proxyDBService.SetProxyFreeStatusAsync(returnedTask.Proxy.Id);
                returnedTask.WorkerId = getTaskData.WorkerId;
                returnedTask.InstanceId = getTaskData.InstanceId;
                await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
                return "No tasks";
            }

            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;

            var countPost = JsonConvert.DeserializeObject<PostingTaskWorkerUsefilDataDTO>(returnedTask.UsefulData).PostPerAccount;
            var task = this.mapper.Map<GetPostingTaskDTO>(returnedTask);

            task.Posts = postList
                .OrderBy(x => Guid.NewGuid().ToString())
                .Where(x => x.InstPostStatus == DB.Models.Post.InstPostStatus.NotPublished || x.InstPostStatus == DB.Models.Post.InstPostStatus.PostingError)
                .Take(countPost)
                .Select(x => x.Post)
                .ToList();

            
            var updatedPosts = postList.Where(x => x.InstPostStatus == DB.Models.Post.InstPostStatus.NotPublished).Take(countPost).ToList();
            foreach (var post in updatedPosts)
            {
                post.InstPostStatus = DB.Models.Post.InstPostStatus.InProcessPublication;
            }

            foreach (var post in task.Posts)
            {
                post.ListPost = null;
            }
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            await this.instPostDBService.UpdateInstPostAsync(updatedPosts);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> CommentingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;
            

            var comments = JsonConvert.DeserializeObject<List<CommentUsefulDataDTO>>(returnedTask.UsefulData);

            List<DBPostComment> updatedComments = new();
            foreach (var comment in comments)
            {
                DBPostComment updatedComment = new DBPostComment();
                try
                {
                    updatedComment = this.postCommentDBService.GetPostCommentById(comment.PostCommentId);
                }
                catch (Exception)
                {
                    returnedTask.Status = DB.Models.TaskStatus.Error;
                    returnedTask.ErrorMessage = "Error parse task";
                    await this.proxyDBService.SetProxyFreeStatusAsync(returnedTask.Proxy.Id);
                    await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
                    return "No tasks";
                }

                updatedComment.CommentStatus = CommentStatus.InProcessPublication;
                updatedComments.Add(updatedComment);
            }
            

            var task = this.mapper.Map<GetCommentingTaskDTO>(returnedTask);
            task.Comments = comments;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            await this.postCommentDBService.UpdatePostCommentAsync(updatedComments);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> LikingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;
            

            var likes = JsonConvert.DeserializeObject<List<LikeUsefulDataDTO>>(returnedTask.UsefulData);

            List<DBPostLikes> updatedLikes = new();
            foreach (var like in likes)
            {
                DBPostLikes updatedlike = new DBPostLikes();
                try
                {
                    updatedlike = this.postLikeDBService.GetPostLikeById(like.PostLikeId);
                }
                catch (Exception)
                {
                    returnedTask.Status = DB.Models.TaskStatus.Error;
                    returnedTask.ErrorMessage = "Error parse task";
                    await this.proxyDBService.SetProxyFreeStatusAsync(returnedTask.Proxy.Id);
                    await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
                    return "No tasks";
                }

                updatedlike.LikeStatus = LikeStatus.InProcessPublication;
                updatedLikes.Add(updatedlike);
            }
            

            var task = this.mapper.Map<GetLikingTaskDTO>(returnedTask);
            task.Likes = likes;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            await this.postLikeDBService.UpdateLikesAsync(updatedLikes);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> FollowingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;
            

            var followIds = JsonConvert.DeserializeObject<List<int>>(returnedTask.UsefulData);
            var follows = new List<FollowUsefulDataDTO>();
            foreach (var followid in followIds)
            {
                DBFollow newFollow = new DBFollow();
                try
                {
                    newFollow = this.followDBService.GetFollowById(followid);
                }
                catch (Exception)
                {
                    returnedTask.Status = DB.Models.TaskStatus.Error;
                    returnedTask.ErrorMessage = "Error parse task";
                    await this.proxyDBService.SetProxyFreeStatusAsync(returnedTask.Proxy.Id);
                    await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
                    return "No tasks";
                }
                
                newFollow.FollowStatus = FollowStatus.InProcessFollow;
                await this.followDBService.UpdateFollowAsync(newFollow);
                follows.Add(mapper.Map<FollowUsefulDataDTO>(newFollow));
            }

            var task = this.mapper.Map<GetFollowingTaskDTO>(returnedTask);
            task.Follows = follows;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> ProfileFillingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;

            var task = this.mapper.Map<GetProfileFillingTask>(returnedTask);
            task.ProfileFillingData = JsonConvert.DeserializeObject<ProfileFillingUsefulDataDTO>(returnedTask.UsefulData);

            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> AdvertLikingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;

            List<DBAdvertPost> listPosts = JsonConvert.DeserializeObject<List<DBAdvertPost>>(returnedTask.UsefulData);
            var task = this.mapper.Map<GetAdvertLikingTaskDTO>(returnedTask);
            task.AdvertPosts = listPosts;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> AdvertCommentingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;

            List<DBAdvertPost> listPosts = JsonConvert.DeserializeObject<List<DBAdvertPost>>(returnedTask.UsefulData);
            var task = this.mapper.Map<GetAdvertCommentingTaskDTO>(returnedTask);
            task.AdvertPosts = listPosts;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> AdvertFollowingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;

            List<DBAdvertAccount> listAccounts = JsonConvert.DeserializeObject<List<DBAdvertAccount>>(returnedTask.UsefulData);
            var task = this.mapper.Map<GetAdvertFollowingTaskDTO>(returnedTask);
            task.AdvertAccounts = listAccounts;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> ParseCloningInformation(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;

            CollectCloneDataUsefuldataDTO usefulData = JsonConvert.DeserializeObject<CollectCloneDataUsefuldataDTO>(returnedTask.UsefulData);
            var task = this.mapper.Map<GetCollectCloningDataTaskDTO>(returnedTask);
            task.CollectData = usefulData;

            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);
            return JsonConvert.SerializeObject(task);
        }
    }
}