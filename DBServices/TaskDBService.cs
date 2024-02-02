using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BASAccountManager.DBServices
{
    public class TaskDBService : ITaskDBService
    {
        private IWorkerTaskDBService workerTaskDBService { get; set; }
        private IProxyDBService proxyDBService { get; set; }
        private AMContext dbcontext;
        private IMapper mapper;

        public TaskDBService(AMContext amcontext, 
            IMapper mapper,
            IWorkerTaskDBService workerTaskDBService,
            IProxyDBService proxyDBService)
        {
            this.dbcontext = amcontext; ;
            this.mapper = mapper;
            this.workerTaskDBService = workerTaskDBService;
            this.proxyDBService = proxyDBService;
        }

        public async Task AddTaskAsync(List<DBTask> newTask)
        {
            foreach (var task in newTask)
            {
                task.CreatedDate = DateTime.Now;
            }
            await this.dbcontext.Task.AddRangeAsync(newTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task AddTaskAsync(DBTask newTask)
        {
            newTask.CreatedDate = DateTime.Now;
            await this.dbcontext.Task.AddAsync(newTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<DBTask> GetTask()
        {
            return this.dbcontext.Task.ToList();
        }

        public JqueryDataTable<TaskDTO> GetTask(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<TaskDTO>();
            data.recordsTotal = this.dbcontext.Task.Count();
            DBTask[] filteredData = Array.Empty<DBTask>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Task.AsQueryable().Where(m => m.ProxyGroup.Contains(searchdata)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ClientTaskName.Contains(searchdata)
                                                || m.AccountGroup.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Task.AsQueryable().Where(m => m.ProxyGroup.Contains(searchdata)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ClientTaskName.Contains(searchdata)
                                                || m.AccountGroup.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<TaskDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<TaskDTO>>(this.dbcontext.Task.AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<TaskDTO>>(this.dbcontext.Task.AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public DBTask GetTaskById(int id)
        {
            return this.dbcontext.Task.Find(id);
        }

        public async Task<bool> RemoveTaskAsync(List<DBTask> removedTask)
        {
            var isFail = true;
            foreach (var item in removedTask)
            {
                var result = await RemoveTaskAsync(item);
                if (result == false)
                {
                    isFail = false;
                }
            }
            return isFail;
        }

        public async Task<bool> RemoveTaskAsync(DBTask removedTask)
        {
            var taskScheduler = this.dbcontext.SchedulerTask.Select(x => x.TaskIds).ToList();
            var Ids = new List<int>();
            foreach (var item in taskScheduler)
            {
                Ids.AddRange(JsonConvert.DeserializeObject<int[]>(item));
            }
            var task = await this.dbcontext.Task.Include(x => x.ListBASExeptions).Where(x => x.Id == removedTask.Id).FirstAsync();
            if (Ids.Contains(task.Id))
            {
                return false;
            }
            else
            {
                this.dbcontext.Task.Remove(task);
                await this.dbcontext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> StartTaskAsync(TaskDTO[] startedTasks)
        {
            foreach (var startedTask in startedTasks)
            {
                switch ((StatusTask)Enum.Parse(typeof(StatusTask), startedTask.Status))
                {
                    case StatusTask.Canceled:
                        startedTask.Status = StatusTask.Added.ToString();
                        await UpdateTaskAsync(this.mapper.Map<DBTask>(startedTask));
                        break;
                    case StatusTask.Completed:
                        startedTask.Status = StatusTask.Added.ToString();
                        var workers = await this.workerTaskDBService.GetWorkerTaskByDBTaskIdAsync(startedTask.Id);
                        await this.workerTaskDBService.RemoveWorkerTaskAsync(workers);
                        await UpdateTaskAsync(this.mapper.Map<DBTask>(startedTask));
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }

        public async Task<bool> StopTaskAsync(TaskDTO[] stoppedTasks)
        {
            foreach (var stoppedTask in stoppedTasks)
            {
                if (stoppedTask.Status == StatusTask.AddingProcess.ToString() || stoppedTask.Status == StatusTask.Added.ToString() || stoppedTask.Status == StatusTask.Performed.ToString())
                {
                    // Устанавливаем статус Canceled головной задаче
                    stoppedTask.Status = StatusTask.Canceled.ToString();
                    await UpdateTaskAsync(this.mapper.Map<DBTask>(stoppedTask));
                    // Получаем лист дочерних воркеров. 
                    var taskWorkers = await this.workerTaskDBService.GetWorkerTaskByDBTaskIdAsync(stoppedTask.Id);
                    // Вытягиваем прокси которые забронированы для работы. Прокси в работе по завершению сами установят себе свободный статус. Устанавилваем прокси свободный статус
                    var updatedProxy = taskWorkers.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).Select(x => x.Proxy).ToList();
                    foreach (var proxy in updatedProxy)
                    {
                        await this.proxyDBService.SetProxyFreeStatusAsync(proxy.Id);
                    }
                    taskWorkers = taskWorkers.Where(x => x.Status != DB.Models.TaskStatus.AtWork).ToList();
                    await this.workerTaskDBService.RemoveWorkerTaskAsync(taskWorkers);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> StartTaskAsync(DBTask[] startedTasks)
        {
            foreach (var startedTask in startedTasks)
            {
                switch (startedTask.Status)
                {
                    case StatusTask.Canceled:
                        startedTask.Status = StatusTask.Added;
                        await UpdateTaskAsync(startedTask);
                        break;
                    case StatusTask.Completed:
                        startedTask.Status = StatusTask.Added;
                        var workers = await this.workerTaskDBService.GetWorkerTaskByDBTaskIdAsync(startedTask.Id);
                        await this.workerTaskDBService.RemoveWorkerTaskAsync(workers);
                        await UpdateTaskAsync(startedTask);
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }

        public async Task<bool> StopTaskAsync(DBTask[] stoppedTasks)
        {
            foreach (var stoppedTask in stoppedTasks)
            {
                if (stoppedTask.Status == StatusTask.AddingProcess || stoppedTask.Status == StatusTask.Added || stoppedTask.Status == StatusTask.Performed)
                {
                    // Устанавливаем статус Canceled головной задаче
                    stoppedTask.Status = StatusTask.Canceled;
                    await UpdateTaskAsync(stoppedTask);
                    // Получаем лист дочерних воркеров. 
                    var taskWorkers = await this.workerTaskDBService.GetWorkerTaskByDBTaskIdAsync(stoppedTask.Id);
                    // Вытягиваем прокси которые забронированы для работы. Прокси в работе по завершению сами установят себе свободный статус. Устанавилваем прокси свободный статус
                    var updatedProxy = taskWorkers.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).Select(x => x.Proxy).ToList();
                    foreach (var proxy in updatedProxy)
                    {
                        await this.proxyDBService.SetProxyFreeStatusAsync(proxy.Id);
                    }
                    taskWorkers = taskWorkers.Where(x => x.Status != DB.Models.TaskStatus.AtWork).ToList();
                    await this.workerTaskDBService.RemoveWorkerTaskAsync(taskWorkers);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public async Task UpdateTaskAsync(List<DBTask> updatedTask)
        {
            this.dbcontext.Task.UpdateRange(updatedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateTaskAsync(DBTask updatedTask)
        {
            this.dbcontext.Task.Update(updatedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
