using AutoMapper;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Task
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskManagerController : ControllerBase
    {
        private IMapper mapper;
        private readonly ILogger<TaskManagerController> logger;
        private ITaskDBService TaskDBService { get; set; }

        public TaskManagerController(ILogger<TaskManagerController> logger, IMapper mapper, ITaskDBService TaskDBService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.TaskDBService = TaskDBService;
        }

        [HttpPost]
        public async Task<string> RegistrationTask(RegistrationTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.TaskType = TaskType.RegistrationAccounts;
            var usefulData = new RegistrationTaskWorkerUsefulDataDTO() { CountAccount = newTask.CountAccount, SMSServiceId = newTask.SMSServiceId };
            task.UsefulData = JsonConvert.SerializeObject(usefulData);
            var taskList = new List<DBTask>();
            taskList.Add(task);
            await this.TaskDBService.AddTaskAsync(taskList);
            return JsonConvert.SerializeObject("true");
        }
    }
}
