using Microsoft.AspNetCore.Mvc;

namespace BASAccountManager.Controllers.FBTask
{
    [ApiController]
    [Route("api/fb/[action]")]
    public class FBTaskController : ControllerBase
    {
        private readonly ILogger<FBTaskController> logger;

        public FBTaskController(ILogger<FBTaskController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public string Task()
        {
            return "asd";
        }
    }
}
