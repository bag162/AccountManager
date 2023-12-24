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
            return await this.InstTaskManager.EndRegistrationTask(newAccount);
        }
    }
}