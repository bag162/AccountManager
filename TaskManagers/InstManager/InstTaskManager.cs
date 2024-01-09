using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.TaskManagers.InstManager.DTO;
using Hangfire.Server;
using Newtonsoft.Json;

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

        public InstTaskManager(ILogger<InstTaskManager> logger,
            IMapper mapper,
            IWorkerTaskDBService workerTaskDBService,
            IInstDBService instDBService,
            ITaskDBService taskDBService,
            IBASExeptionDBService BASExeption,
            IInstPostDBService instPostDBService)
        {
            this.Logger = logger;
            this.mapper = mapper;
            this.workerTaskDBService = workerTaskDBService;
            this.instDBService = instDBService;
            this.taskDBService = taskDBService;
            this.BASExeption = BASExeption;
            this.instPostDBService = instPostDBService;
        }

        public async Task<string> GetTaskAsync(GetTaskDTO getTaskData)
        {
            var allTasks = await this.workerTaskDBService.GetWorkerTasksAsync();

            if (allTasks.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).Count() == 0)
                return "No tasks";

            var returnedTask = allTasks.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).First();

            switch (returnedTask.TaskType)
            {
                case TaskType.RegistrationAccounts:
                    return await RegistrationTaskImplAsync(returnedTask, getTaskData);
                case TaskType.AuthorizationAccounts:
                    return await AuthorizationTaskImplAsync(returnedTask, getTaskData);
                case TaskType.Posting:
                    return await PostingTaskImplAsync(returnedTask, getTaskData);
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
            taskWorker.Proxy.ProxyStatus = ProxyStatus.Free;

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
            taskWorker.Proxy.ProxyStatus = ProxyStatus.Free;
            taskWorker.Status = DB.Models.TaskStatus.Completed;

            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);

        }

        public async Task<string> EndPostingTaskAsync(EndPostingTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.workerId);
            taskWorker.Proxy.ProxyStatus = ProxyStatus.Free;
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> IntermediateEndPostingTaskAsync(IntermediateEndPostingTaskDTO endData)
        {
            var worker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.workerId);
            var instpost = this.instPostDBService.GetInstPospsByAccount(worker.AccountId).Where(x => x.PostId == endData.postId).First();
            instpost.PostURI = endData.postURI;
            instpost.InstPostStatus = DB.Models.Post.InstPostStatus.Published;
            await this.instPostDBService.UpdateInstPostAsync(instpost);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorRegistrationTaskAsync(RegistrationTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();
            workerTask.Proxy.ProxyStatus = ProxyStatus.Free;
            // Изменяем статус головной задачи на "В процессе добавления", т.к. необходимо восполнить неудачно завершившуюся задачу новой.
            workerTask.Task.Status = StatusTask.AddingProcess;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(workerTask);
            return JsonConvert.SerializeObject(true);
        }

        public async Task<string> ErrorAuthorizationTaskAsync(AuthorizationTaskErrorType error, int workerId)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(workerId);
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = error.ToString();
            workerTask.Proxy.ProxyStatus = ProxyStatus.Free;
            switch (error)
            {
                case AuthorizationTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
                case AuthorizationTaskErrorType.IncorrectAuthData:
                    workerTask.Account.AccountStatus = AccountStatus.IncorrectCredentionalData;
                    break;
            }

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
                    workerTask.Proxy.ProxyStatus = ProxyStatus.Free;
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
            workerTask.Proxy.ProxyStatus = ProxyStatus.Free;

            switch (error)
            {
                case PostingTaskErrorType.FullBan:
                    workerTask.Account.AccountStatus = AccountStatus.Banned;
                    break;
            }

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

            await this.BASExeption.AddAsync(exeption);
            workerTask.Proxy.ProxyStatus = ProxyStatus.Free;
            workerTask.Status = DB.Models.TaskStatus.Error;
            workerTask.ErrorMessage = "GlobalExeption";
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
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);

            var task = this.mapper.Map<GetAuthorizationTaskDTO>(returnedTask);

            return JsonConvert.SerializeObject(task);
        }

        private async Task<string> PostingTaskImplAsync(DBWorkerTask returnedTask, GetTaskDTO getTaskData)
        {
            returnedTask.Status = DB.Models.TaskStatus.AtWork;
            returnedTask.Proxy.ProxyStatus = ProxyStatus.InWork;
            returnedTask.WorkerId = getTaskData.WorkerId;
            returnedTask.InstanceId = getTaskData.InstanceId;
            await this.workerTaskDBService.UpdateWorkerTaskAsync(returnedTask);

            var countPost = JsonConvert.DeserializeObject<PostingTaskWorkerUsefilDataDTO>(returnedTask.UsefulData).PostPerAccount;
            var task = this.mapper.Map<GetPostingTaskDTO>(returnedTask);
            var postList = this.instPostDBService.GetInstPospsByAccount(returnedTask.AccountId);

            task.Posts = postList
                .Where(x => x.InstPostStatus == DB.Models.Post.InstPostStatus.NotPublished)
                .Take(countPost)
                .Select(x => x.Post)
                .ToList();

            return JsonConvert.SerializeObject(task);
        }
    }
}