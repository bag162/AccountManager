using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.TaskManagers.InstManager;
using Microsoft.AspNetCore.Mvc;

namespace BASAccountManager.Controllers.BASTask
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BASTaskErrorController : ControllerBase
    {
        private IMapper mapper;
        private readonly ILogger<BASTaskErrorController> logger;
        private InstTaskManager InstTaskManager;

        public BASTaskErrorController(IMapper mapper, ILogger<BASTaskErrorController> logger, InstTaskManager InstTaskManager)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.InstTaskManager = InstTaskManager;
        }

        [HttpPost]
        public async Task<string> RegistrationTaskError(RegistrationTaskTypeError error)
        {

        }
    }
}
