using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.BackgroundTask
{
    public class StatusMonitor
    {
        private ILogger<StatusMonitor> logger;
        private ISMSServiceDB SMSServiceDB { get; set; }
        private IProxyDBService proxyDBService { get; set; }
        private ITaskDBService taskDbService { get; set; }
        private IWorkerTaskDBService workerTaskDbService { get; set; }

        public StatusMonitor(ITaskDBService taskDbService, 
            IWorkerTaskDBService workerTaskDbService, 
            IProxyDBService proxyDBService, 
            ISMSServiceDB SMSServiceDB, 
            ILogger<StatusMonitor> logger)
        {
            this.taskDbService = taskDbService;
            this.workerTaskDbService = workerTaskDbService;
            this.proxyDBService = proxyDBService;
            this.SMSServiceDB = SMSServiceDB;
            this.logger = logger;
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
                

                proxy.ProxyStatus = DB.Models.ProxyStatus.Free;
                await this.proxyDBService.UpdateProxyAsync(proxy);
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
    }
}