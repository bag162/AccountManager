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
        public InstTaskManager(ILogger<InstTaskManager> logger, 
            IMapper mapper, 
            IWorkerTaskDBService workerTaskDBService, 
            IInstDBService instDBService,
            ITaskDBService taskDBService)
        {
            this.Logger = logger;
            this.mapper = mapper;
            this.workerTaskDBService = workerTaskDBService;
            this.instDBService = instDBService;
            this.taskDBService = taskDBService;
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
            }
            this.Logger.LogWarning("Skip switch on InstTaskManager");
            return "No tasks";
        }

        public async Task<string> EndRegistrationTask(EndRegistrationTaskDTO endData)
        {
            var taskWorker = await this.workerTaskDBService.GetWorkerByIdAsync(endData.TaskWorkerId);
            DBInstagramAccount newAccount = this.mapper.Map<DBInstagramAccount>(endData);
            var accountId = await this.instDBService.AddInstAccountsAsync(newAccount, taskWorker.Task.AccountGroup);

            taskWorker.InstAccountId = accountId;
            taskWorker.Status = DB.Models.TaskStatus.Completed;
            taskWorker.Proxy.ProxyStatus = ProxyStatus.Free;

            await this.workerTaskDBService.UpdateWorkerTaskAsync(taskWorker);
            return JsonConvert.SerializeObject("true");
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
    }
}