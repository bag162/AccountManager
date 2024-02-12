using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.InstTask
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "/taskmanager/taskdata,admin")]
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
        public async Task<string> Delete([FromBody] TaskDTO[] deletetTasks)
        {
            var isFail = true;
            foreach (var deletetTask in deletetTasks)
            {
                if (deletetTask.Status == StatusTask.Canceled.ToString() || deletetTask.Status == StatusTask.Completed.ToString())
                {
                    var result = await this.TaskDBService.RemoveTaskAsync(this.mapper.Map<DBTask>(deletetTask));
                    if (result == false)
                    {
                        isFail = false;
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(false);
                }
            }
            return JsonConvert.SerializeObject(isFail);
        }

        [HttpPut]
        public async Task<string> Stop([FromBody] TaskDTO[] stoppedTasks)
        {
            return JsonConvert.SerializeObject(await this.TaskDBService.StopTaskAsync(stoppedTasks));
        }

        [HttpPut]
        public async Task<string> Start([FromBody] TaskDTO[] startedTasks)
        {
            var result = JsonConvert.SerializeObject(await this.TaskDBService.StartTaskAsync(startedTasks));
            BackgroundJob.Enqueue<HangFireTaskManager>((method) => method.TaskParser());
            return result;
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