using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.InstTask
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        private IMapper mapper;
        private readonly ILogger<TaskController> logger;
        private ITaskDBService TaskDBService { get; set; }
        private IWorkerTaskDBService WorkerTaskDBService { get; set; }
        private IProxyDBService ProxyDBService { get; set; }

        public TaskController(ILogger<TaskController> logger, IMapper mapper, ITaskDBService TaskDBService, IWorkerTaskDBService WorkerTaskDBService, IProxyDBService ProxyDBService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.TaskDBService = TaskDBService;
            this.WorkerTaskDBService = WorkerTaskDBService;
            this.ProxyDBService = ProxyDBService;
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] TaskDTO deletetTask)
        {
            if (deletetTask.Status == StatusTask.Canceled.ToString() || deletetTask.Status == StatusTask.Completed.ToString())
            {
                await this.TaskDBService.RemoveTaskAsync(this.mapper.Map<DBTask>(deletetTask));
                return JsonConvert.SerializeObject("True");
            }
            else
            {
                return JsonConvert.SerializeObject("False");
            }
        }

        [HttpPut]
        public async Task<string> Stop([FromBody] TaskDTO stoppedTask)
        {
            if(stoppedTask.Status == StatusTask.AddingProcess.ToString() || stoppedTask.Status == StatusTask.Added.ToString() || stoppedTask.Status == StatusTask.Performed.ToString())
            {
                // Устанавливаем статус Canceled головной задаче
                stoppedTask.Status = StatusTask.Canceled.ToString();
                await this.TaskDBService.UpdateTaskAsync(this.mapper.Map<DBTask>(stoppedTask));
                // Получаем лист дочерних воркеров. 
                var taskWorkers = await this.WorkerTaskDBService.GetWorkerTaskByDBTaskIdAsync(stoppedTask.Id);
                // Удаляем воркеры которые не выполннялись. Позже их добавит обработчик при запуске задачи.
                var deletedWorkers = taskWorkers.Where(x => x.Status == DB.Models.TaskStatus.NotTaken).ToList();
                await this.WorkerTaskDBService.RemoveWorkerTaskAsync(deletedWorkers);
                // Вытягиваем прокси которые забронированы для работы. Прокси в работе по завершению сами установят себе свободный статус. Устанавилваем прокси свободный статус
                var updatedProxy = taskWorkers.Where(x => x.Proxy.ProxyStatus == ProxyStatus.BookedForWork).Select(x => x.Proxy).ToList();
                foreach (var proxy in updatedProxy)
                {
                    proxy.ProxyStatus = ProxyStatus.Free;
                }
                // Обновляем прокси
                await this.ProxyDBService.UpdateProxyAsync(updatedProxy);
                return JsonConvert.SerializeObject("True");
            }
            else
            {
                return JsonConvert.SerializeObject("False");
            }
        }

        [HttpPut]
        public async Task<string> Start([FromBody] TaskDTO stoppedTask)
        {
            switch ((StatusTask)Enum.Parse(typeof(StatusTask), stoppedTask.Status))
            {
                case StatusTask.Canceled:
                    stoppedTask.Status = StatusTask.Added.ToString();
                    await this.TaskDBService.UpdateTaskAsync(this.mapper.Map<DBTask>(stoppedTask));
                    break;
                case StatusTask.Completed:
                    stoppedTask.Status = StatusTask.Added.ToString();
                    var workers = await this.WorkerTaskDBService.GetWorkerTaskByDBTaskIdAsync(stoppedTask.Id);
                    await this.WorkerTaskDBService.RemoveWorkerTaskAsync(workers);
                    await this.TaskDBService.UpdateTaskAsync(this.mapper.Map<DBTask>(stoppedTask));
                    break;
                default:
                    return JsonConvert.SerializeObject("false");
            }
            return JsonConvert.SerializeObject("true");
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<TaskDTO> returnedData = this.TaskDBService.GetTask(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }
    }
}