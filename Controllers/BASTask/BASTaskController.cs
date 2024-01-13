using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.TaskManagers.InstManager;
using BASAccountManager.TaskManagers.InstManager.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BASAccountManager.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BASTaskController
    {
        private IMapper mapper;
        private readonly ILogger<BASTaskController> logger;
        private InstTaskManager InstTaskManager;

        public BASTaskController(IMapper mapper, ILogger<BASTaskController> logger, InstTaskManager InstTaskManager)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.InstTaskManager = InstTaskManager;
        }

        [HttpGet]
        public async Task<string> Get(int WorkerId, string InstanceId)
        {
            return await InstTaskManager.GetTaskAsync(new GetTaskDTO { WorkerId = WorkerId, InstanceId = InstanceId });
        }

        [HttpPost]
        public async Task<string> EndRegistrationTask([FromBody] EndRegistrationTaskDTO newAccount)
        {
            return await this.InstTaskManager.EndRegistrationTaskAsync(newAccount);
        }

        [HttpPost]
        public async Task<string> EndAuthorizationTask(EndAuthorizationTaskDTO data)
        {
            return await this.InstTaskManager.EndAuthorizationTaskAsync(data);
        }

        [HttpPost]
        public async Task<string> IntermediateEndPostingTask(IntermediateEndPostingTaskDTO data)
        {
            return await this.InstTaskManager.IntermediateEndPostingTaskAsync(data);
        }

        [HttpPost]
        public async Task<string> EndPostingTask(EndPostingTaskDTO data)
        {
            return await this.InstTaskManager.EndPostingTaskAsync(data);
        }

        [HttpPost]
        public async Task<string> IntermediateEndCommentingTask(IntermediateEndCommentingTaskDTO data)
        {
            return await this.InstTaskManager.IntermediateEndCommentingTask(data);
        }

        [HttpPost]
        public async Task<string> EndCommentingTask(EndCommentingTaskDTO data)
        {
            return await this.InstTaskManager.EndCommentingTaskAsync(data);
        }

        [HttpPost]
        public async Task<string> IntermediateEndLikingTask(IntermediateEndLikingTaskDTO data)
        {
            return await this.InstTaskManager.IntermediateEndLikingTask(data);
        }

        [HttpPost]
        public async Task<string> EndLikingTask(EndLikingTaskDTO data)
        {
            return await this.InstTaskManager.EndLikingTaskAsync(data);
        }

        [HttpPost]
        public async Task<string> EndFollowingTask(EndFollowingTaskDTO data)
        {
            return await this.InstTaskManager.EndFollowingTaskTasync(data);
        }

        [HttpPost]
        public async Task<string> IntermediateEndFollowingTask(IntermediateEndFollowingTaskDTO data)
        {
            return await this.InstTaskManager.IntermediateEndFollowingTask(data);
        }
    }
}