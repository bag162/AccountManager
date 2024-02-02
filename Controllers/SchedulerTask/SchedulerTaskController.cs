using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.Controllers.SchedulerTask.DTO;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.SchedulerTask
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SchedulerTaskController : ControllerBase
    {
        private ISchedulerTaskDBService schedulerTaskDBService { get; set; }
        public SchedulerTaskController(ISchedulerTaskDBService schedulerTaskDBService)
        {
            this.schedulerTaskDBService = schedulerTaskDBService;
        }

        [Route("{id:int}")]
        [Authorize(Roles = "/taskscheduler/view,admin")]
        [HttpGet]
        public async Task<string> Get(int id)
        {
            var schedulerTask = await this.schedulerTaskDBService.GetSchedulerTaskByIdAsync(id);
            return JsonConvert.SerializeObject(schedulerTask);
        }

        [HttpGet]
        [Authorize(Roles = "/taskscheduler,admin")]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<SchedulerTaskDTO> returnedData = this.schedulerTaskDBService.GetSchedulerTask(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpPost]
        [Authorize(Roles = "/taskscheduler,admin")]
        public async Task<string> Post(AddSchedulerTaskDTO data)
        {
            var result = await this.schedulerTaskDBService.AddSchedulerTaskAsync(data);
            return JsonConvert.SerializeObject(result);
        }

        [HttpDelete]
        [Authorize(Roles = "/taskscheduler,admin")]
        public async Task<string> Delete(int[] ids)
        {
            foreach (var item in ids)
            {
                await this.schedulerTaskDBService.DeleteSchedulerTaskAsync(item);
            }
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "/taskscheduler/add,admin")]
        public async Task<string> Start(int[] Ids)
        {
            await this.schedulerTaskDBService.StartSchedulerTasksAsync(Ids);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "/taskscheduler,admin")]
        public async Task<string> Stop(int[] Ids)
        {
            await this.schedulerTaskDBService.StopSchedulerTasksAsync(Ids);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPut]
        [Authorize(Roles = "/taskscheduler/view,admin")]
        public async Task<string> Put([FromBody] UpdateSchedulerTaskDTO data)
        {
            await this.schedulerTaskDBService.UpdateAsync(data);
            return JsonConvert.SerializeObject(true);
        }
    }
}