using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
        private IWorkerTaskDBService WorkerTaskDBService { get; set; }

        public TaskManagerController(ILogger<TaskManagerController> logger, IMapper mapper, ITaskDBService TaskDBService, IWorkerTaskDBService WorkerTaskDBService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.TaskDBService = TaskDBService;
            this.WorkerTaskDBService = WorkerTaskDBService;
        }

        [Route("{id:int}")]
        [HttpGet]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "/taskmanager/workertaskdata")]
        public string Get(int start, int length, int draw, int id)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<WorkerTaskDTO> returnedData = this.WorkerTaskDBService.GetWorkerTasks(start, length, searchData.First(), id);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "/taskmanager/registration")]
        public async Task<string> RegistrationTask(RegistrationTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.TaskType = TaskType.RegistrationAccounts;
            task.ClientTaskName = newTask.ClientTaskName;
            var usefulData = new RegistrationTaskWorkerUsefulDataDTO() { CountAccount = newTask.CountAccount, SMSServiceId = newTask.SMSServiceId, EmailServiceId = newTask.EmailServiceId, RegistrationVerifyResoursesType = newTask.ResoursesType };
            task.UsefulData = JsonConvert.SerializeObject(usefulData);
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "/taskmanager/authorization")]
        public async Task<string> AuthorizationTask([FromBody] AuthorizationTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.ClientTaskName;
            task.TaskType = TaskType.AuthorizationAccounts;

            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "/taskmanager/posting")]
        public async Task<string> PostingTask([FromBody] PostingTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.ClientTaskName;
            task.TaskType = TaskType.Posting;
            task.UsefulData = JsonConvert.SerializeObject(new PostingTaskWorkerUsefilDataDTO() { PostPerAccount = newTask.PostPerAccount });

            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "/taskmanager/commenting")]
        public async Task<string> CommentingTask([FromBody] CommentingTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.ClientTaskName;
            task.TaskType = TaskType.Commenting;
            task.UsefulData = JsonConvert.SerializeObject(new CommentingTaskWorkerUsefulDataDTO() { CommentsPerAccount = newTask.CommentsPerAccount, PostGroup = newTask.PostGroup });
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "/taskmanager/liking")]
        public async Task<string> LikingTask([FromBody] LikingTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.ClientTaskName;
            task.TaskType = TaskType.Liking;
            task.UsefulData = JsonConvert.SerializeObject(new LikingTaskWorkerUsefulDatadTO() { LikesPerAccount = newTask.LikesPerAccount, PostGroup = newTask.PostGroup });
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "/taskmanager/folowing")]
        public async Task<string> FollowingTask([FromBody] FollowingTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.ClientTaskName;
            task.TaskType = TaskType.Following;
            task.UsefulData = JsonConvert.SerializeObject(new FollowingTaskWorkerUsefulDataDTO() 
            { 
                FollowsPerAccount = newTask.FollowsPerAccount,
                AccountGroupForSubscription = newTask.AccountGroupForSubscription,
                RequiredFollowersPerAccount = newTask.RequiredFollowersPerAccount
            });
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }
    }
}