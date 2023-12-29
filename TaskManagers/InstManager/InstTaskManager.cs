using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.TaskManagers.InstManager.DTO;
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

        public InstTaskManager(ILogger<InstTaskManager> logger,
            IMapper mapper,
            IWorkerTaskDBService workerTaskDBService,
            IInstDBService instDBService,
            ITaskDBService taskDBService,
            IBASExeptionDBService BASExeption)
        {
            this.Logger = logger;
            this.mapper = mapper;
            this.workerTaskDBService = workerTaskDBService;
            this.instDBService = instDBService;
            this.taskDBService = taskDBService;
            this.BASExeption = BASExeption;
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

            taskWorker.InstAccountId = accountId;
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
            taskWorker.InstAccount.ProfileLink = endData.ProfileLink;
            taskWorker.InstAccount.Name = endData.Name;
            taskWorker.InstAccount.Surname = endData.Surname;
            taskWorker.InstAccount.Email = endData.Email;
            taskWorker.InstAccount.PhoneNumber = endData.PhoneNumber;
            taskWorker.InstAccount.AccountStatus = AccountStatus.Authorized;
            taskWorker.InstAccount.InstanceId = taskWorker.InstanceId;
            taskWorker.Proxy.ProxyStatus = ProxyStatus.Free;
            taskWorker.Status = DB.Models.TaskStatus.Completed;

            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
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
                    workerTask.InstAccount.AccountStatus = AccountStatus.Banned;
                    break;
                case AuthorizationTaskErrorType.IncorrectAuthData:
                    workerTask.InstAccount.AccountStatus = AccountStatus.IncorrectCredentionalData;
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

        public async Task<string> ErrorGlobalAsync(GlobalErrorDTO error)
        {
            var workerTask = await this.workerTaskDBService.GetWorkerByIdAsync(error.workerId);
            DBBASExeption exeption = new DBBASExeption
            {
                AccountId = workerTask.InstAccountId,
                DBTaskId = workerTask.DBTaskId,
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
    }
}