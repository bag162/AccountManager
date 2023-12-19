using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Newtonsoft.Json;

namespace BASAccountManager.BackgroundTask
{
    public class AssignmentWriter
    {
        private ILogger<AssignmentWriter> logger;
        private ISMSServiceDB SMSServiceDB;
        private IProxyDBService proxyDBService;
        private ITaskDBService taskDbService { get; set; }
        private IWorkerTaskDBService workerTaskDbService { get; set; }

        public AssignmentWriter(ITaskDBService taskDbService, IWorkerTaskDBService workerTaskDbService, IProxyDBService proxyDBService, ISMSServiceDB SMSServiceDB, ILogger<AssignmentWriter> logger)
        {
            this.taskDbService = taskDbService;
            this.workerTaskDbService = workerTaskDbService;
            this.proxyDBService = proxyDBService;
            this.SMSServiceDB = SMSServiceDB;
            this.logger = logger;
        }

        public async Task TaskParser()
        {
            var addedTask = this.taskDbService.GetTask().Where(x => x.Status == StatusTask.Added || x.Status == StatusTask.AddingProcess);
            foreach (var newTask in addedTask)
            {
                await this.GenWorkerTask(newTask);
            }
        }

        private async Task GenWorkerTask(DBTask task)
        {
            switch (task.TaskType)
            {
                case TaskType.RegistrationAccounts:
                    task.Status = StatusTask.AddingProcess;
                    await this.taskDbService.UpdateTaskAsync(task);

                    var addedTask = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);
                    List<DBWorkerTask> workerTaskList = new();
                    RegistrationTaskWorkerUsefulDataDTO usefulData = JsonConvert.DeserializeObject<RegistrationTaskWorkerUsefulDataDTO>(task.UsefulData);
                    int countAddTask;
                    if (addedTask.Count == 0)
                    {
                        countAddTask = usefulData.CountAccount;
                    }
                    else
                    {
                        countAddTask = usefulData.CountAccount - addedTask.Count;
                    }
                    for (int i = 0; i < countAddTask; i++)
                    {
                        var proxy = await GetFreeProxyByGroup(task.ProxyGroup);
                        if (proxy == null)
                        {
                            break;
                        }
                        var smsService = this.SMSServiceDB.GetSMSServiceById(usefulData.SMSServiceId);
                        var workerTask = new DBWorkerTask() {
                            Status = DB.Models.TaskStatus.NotTaken,
                            TaskName = TaskType.RegistrationAccounts.ToString(),
                            DBTaskId = task.Id,
                            ProxyId = proxy.Id,
                            UsefulData = JsonConvert.SerializeObject(new RegistrationClientTaskWorkerUsefulDataDTO() { SMSServiceData = smsService })
                        };
                        workerTaskList.Add(workerTask);

                        if (i == countAddTask-1)
                        {
                            task.Status = StatusTask.Performed;
                            await this.taskDbService.UpdateTaskAsync(task);
                        }
                    }

                    await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
                    break;
                default:
                    break;
            }
        }

        private async Task<DBProxy> GetFreeProxyByGroup(string group)
        {
            var proxy = this.proxyDBService.GetProxyByGroup(group);
            if (proxy.Where(x => x.ProxyStatus == ProxyStatus.Free).Count() == 0)
            {
                return null;
            }
            else
            {
                var returnedProxy = proxy.Where(x => x.ProxyStatus == ProxyStatus.Free).First();
                returnedProxy.ProxyStatus = ProxyStatus.BookedForWork;
                await this.proxyDBService.UpdateProxyAsync(returnedProxy);
                return returnedProxy;
            }
        }
    }
}