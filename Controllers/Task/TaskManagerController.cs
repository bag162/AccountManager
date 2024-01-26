using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

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
        [Authorize(Roles = "/taskmanager/workertaskdata,admin")]
        public string Get(int start, int length, int draw, int id)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<WorkerTaskDTO> returnedData = this.WorkerTaskDBService.GetWorkerTasks(start, length, searchData.First(), id);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpPost]
        [Authorize(Roles = "/taskmanager/registration,admin")]
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
        [Authorize(Roles = "/taskmanager/authorization,admin")]
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
        [Authorize(Roles = "/taskmanager/posting,admin")]
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
        [Authorize(Roles = "/taskmanager/commenting,admin")]
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
        [Authorize(Roles = "/taskmanager/liking,admin")]
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
        [Authorize(Roles = "/taskmanager/folowing,admin")]
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

        [HttpPost]
        [Authorize(Roles = "/taskmanager/profilefilling,admin")]
        public async Task<string> FillingProfileTask([FromBody] FillingProfileTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.ClientTaskName;
            task.TaskType = TaskType.FillingProfile;
            task.UsefulData = JsonConvert.SerializeObject(new FillingProfileTaskWorkerUsefulDataDTO() { FillingProfileName = newTask.FillingProfileName });
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "/taskmanager/advertliking,admin")]
        public async Task<string> AdvertLiking([FromBody] AdvertLikingTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.TaskName;
            task.TaskType = TaskType.AdvertLiking;
            task.UsefulData = JsonConvert.SerializeObject(new AdvertLikingTaskWorkerUsefulDatadTO() 
            { 
                AdvertPostGroup = newTask.AdvertPostGroup, 
                LikeIfPostCommentedPreviously = newTask.LikeIfPostCommentedPreviously, 
                LikesPerAccount = newTask.LikesPerAccount  
            });
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "/taskmanager/advertcommenting,admin")]
        public async Task<string> AdvertCommenting([FromBody] AdvertCommentingTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.TaskName;
            task.TaskType = TaskType.AdvertCommenting;
            task.UsefulData = JsonConvert.SerializeObject(new AdvertCommentingTaskWorkerUsefulDatadTO()
            {
                AdvertPostGroup = newTask.AdvertPostGroup,
                CommentIfPostLikedPreviously = newTask.CommentIfPostLikedPreviously,
                CommentsPerAccount = newTask.CommentsPerAccount,
                CommentsGroup = newTask.CommentsGroup
            });
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "/taskmanager/advertfollowing,admin")]
        public async Task<string> AdvertFollowing([FromBody] AdvertFollowingTaskDTO newTask)
        {
            var task = new DBTask();
            task.Status = StatusTask.Added;
            task.ProxyGroup = newTask.ProxyGroup;
            task.AccountGroup = newTask.AccountGroup;
            task.ClientTaskName = newTask.TaskName;
            task.TaskType = TaskType.AdvertFollowing;
            task.UsefulData = JsonConvert.SerializeObject(new AdvertFollowingTaskWorkerUsefulDatadTO()
            {
                AdvertAccountGroup = newTask.AdvertAccountGroup,
                FollowsPerAccount = newTask.FollowsPerAccount
            });
            await this.TaskDBService.AddTaskAsync(task);
            return JsonConvert.SerializeObject(true);
        }
    }
}