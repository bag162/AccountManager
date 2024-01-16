using AutoMapper;
using BASAccountManager.Controllers.Instagram.DTO;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Instagram
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    [Authorize(Roles = "/InstGroup")]
    public class InstGroupController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IInstDBService InstDbService { get; set; }
        private readonly ILogger<InstGroupController> logger;

        public InstGroupController(ILogger<InstGroupController> logger, IMapper mapper, IInstDBService InstDbService)
        {
            this.mapper = mapper;
            this.InstDbService = InstDbService;
            this.logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(this.InstDbService.GetAllGroups());
        }

        [HttpPost]
        public async Task<string> Post([FromBody] InstGroupDTO[] newGroup)
        {
            await this.InstDbService.AddGroupAsync(new DB.Models.DBInstAccountGroup() { Name = newGroup.First().GroupName });
            return JsonConvert.SerializeObject(true);
        }

    }
}
