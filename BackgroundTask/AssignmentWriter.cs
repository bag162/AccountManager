using BASAccountManager.Controllers.BASTask.DTO;
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
        private IEmailDBService emailDBService { get; set; }
        private IInstDBService instDBService { get; set; }

        public AssignmentWriter(ITaskDBService taskDbService, 
            IWorkerTaskDBService workerTaskDbService, 
            IProxyDBService proxyDBService, 
            ISMSServiceDB SMSServiceDB, 
            ILogger<AssignmentWriter> logger,
            IEmailDBService emailDBService,
            IInstDBService instDBService)
        {
            this.taskDbService = taskDbService;
            this.workerTaskDbService = workerTaskDbService;
            this.proxyDBService = proxyDBService;
            this.SMSServiceDB = SMSServiceDB;
            this.logger = logger;
            this.emailDBService = emailDBService;
            this.instDBService = instDBService;
        }

        public async Task TaskParserAsync()
        {
            var addedTask = this.taskDbService.GetTask().Where(x => x.Status == StatusTask.Added || x.Status == StatusTask.AddingProcess);
            foreach (var newTask in addedTask)
            {
                await this.GenWorkerTaskAsync(newTask);
            }
        }

        private async Task GenWorkerTaskAsync(DBTask task)
        {
            switch (task.TaskType)
            {
                case TaskType.RegistrationAccounts:
                    await ParseRegistrationTask(task);
                    break;
                case TaskType.AuthorizationAccounts:
                    await ParseAuthorizationTask(task);
                    break;
                default:
                    break;
            }
        }

        private async Task<DBProxy> GetFreeProxyByGroupAsync(string group)
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

        private async Task ParseRegistrationTask(DBTask task)
        {
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
                // Убираем задачи которые завершились с ошибкой, т.к. в них не было зарегестрировано аккаунтов
                countAddTask = usefulData.CountAccount - addedTask.Where(x => x.Status != DB.Models.TaskStatus.Error).Count();
            }
            for (int i = 0; i < countAddTask; i++)
            {
                var proxy = await GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }

                var workerTask = new DBWorkerTask()
                {
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.RegistrationAccounts,
                    DBTaskId = task.Id,
                    ProxyId = proxy.Id
                };

                switch (usefulData.RegistrationVerifyResoursesType)
                {
                    case RegistrationVerifyResoursesType.EmailService:
                        var emailService = this.emailDBService.GetEmailById((int)usefulData.EmailServiceId);
                        workerTask.UsefulData = JsonConvert.SerializeObject(new RegistrationClientTaskWorkerUsefulDataEmailServiceDTO() { EmailServiceData = emailService });
                        workerTask.RegistrationVerifyResoursesType = RegistrationVerifyResoursesType.EmailService;
                        break;
                    case RegistrationVerifyResoursesType.SMSService:
                        var smsService = this.SMSServiceDB.GetSMSServiceById((int)usefulData.SMSServiceId);
                        workerTask.UsefulData = JsonConvert.SerializeObject(new RegistrationClientTaskWorkerUsefulDataSMSServiceDTO() { SMSServiceData = smsService });
                        workerTask.RegistrationVerifyResoursesType = RegistrationVerifyResoursesType.SMSService;
                        break;
                }

                workerTaskList.Add(workerTask);

                if (i == countAddTask - 1)
                {
                    task.Status = StatusTask.Performed;
                    await this.taskDbService.UpdateTaskAsync(task);
                }
            }

            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
        }

        private async Task ParseAuthorizationTask(DBTask task)
        {
            task.Status = StatusTask.AddingProcess;
            await this.taskDbService.UpdateTaskAsync(task);
            List<DBWorkerTask> workerTaskList = new();

            // Получаем список всех добавленных задач на авторизацию
            var addedTask = await this.workerTaskDbService.GetWorkerTaskByDBTaskIdAsync(task.Id);
            
            // Получае список всех аккаунтов в группе на авторизацию
            var addedList = await this.instDBService.GetInstAccountsByGroupAsync(task.AccountGroup);

            // Создаем список на удаление задач из списка на добавление. Если у задачи Status != Error, то удаляем эти аккаунты
            var deleteOutAddedList = addedTask.Where(x => x.Status != DB.Models.TaskStatus.Error).Select(x => x.InstAccount).ToList();
            // Удаляем из списка на добавление аккаунтов все аккаунты, у задач которых статус != Error
            var listToAdd = new List<DBInstagramAccount>(); listToAdd.AddRange(addedList);
            if (deleteOutAddedList != null && deleteOutAddedList.Count != 0 && addedList != null && addedList.Count != 0)
            {
                foreach (var accToAdd in addedList)
                {
                    if (deleteOutAddedList.Where(x => x.Id == accToAdd.Id).Count() != 0)
                    {
                        listToAdd.Remove(accToAdd);
                    }
                }
            }

            
            // Создаем список, который будет сигнализивать что мы обратали все аккаунты, что бы установить статус Performed
            var checkFullAddList = new List<DBInstagramAccount>();
            checkFullAddList.AddRange(listToAdd);

            foreach (var newAccount in listToAdd)
            {
                // Если статус у аккаунте не равен "NotAuthorized", то удаяем его из сигнального списка и пропусаем итерацию
                if (newAccount.AccountStatus != AccountStatus.NotAuthorized)
                {
                    checkFullAddList.Remove(newAccount);
                    continue;
                }
                    
                var proxy = await this.GetFreeProxyByGroupAsync(task.ProxyGroup);
                if (proxy == null)
                {
                    break;
                }
                var newTask = new DBWorkerTask()
                {
                    DBTaskId = task.Id,
                    Status = DB.Models.TaskStatus.NotTaken,
                    TaskType = TaskType.AuthorizationAccounts,
                    InstAccountId = newAccount.Id,
                    ProxyId = proxy.Id
                };
                checkFullAddList.Remove(newAccount);
                workerTaskList.Add(newTask);
            }

            // Если все аккаунты добавлены, то обновляем статус головной задачи
            if (checkFullAddList.Count == 0)
            {
                task.Status = StatusTask.Performed;
                await this.taskDbService.UpdateTaskAsync(task);
            }
            await this.workerTaskDbService.AddWorkerTaskAsync(workerTaskList);
            return;
        }
    }
}