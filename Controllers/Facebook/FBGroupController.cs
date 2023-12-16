using AutoMapper;
using BASAccountManager.Controllers.Facebook.DTO;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Facebook
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FBGroupController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IFBDBService FbDbService { get; set; }
        private readonly ILogger<FBGroupController> logger;

        public FBGroupController(ILogger<FBGroupController> logger, IMapper mapper, IFBDBService FbDbService)
        {
            this.mapper = mapper;
            this.FbDbService = FbDbService;
            this.logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(this.FbDbService.GetAllGroups());
        }

        [HttpPost]
        public async Task<string> Post([FromBody] FBGroupDTO[] newGroup)
        {
            await this.FbDbService.AddGroupAsync(new DB.Models.DBFBAccountGroup() { Name = newGroup.First().GroupName });
            return JsonConvert.SerializeObject(true);
        }

    }
}
