using AutoMapper;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Proxy
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProxyGroupController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IProxyDBService proxyDBService { get; set; }
        private readonly ILogger<ProxyGroupController> logger;

        public ProxyGroupController(ILogger<ProxyGroupController> logger, IProxyDBService proxyDBService, IMapper mapper)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.proxyDBService = proxyDBService;
        }

        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(this.proxyDBService.GetAllGroups());
        }

        [HttpPost]
        public async Task<string> Post([FromBody] ProxyGroupDTO[] newGroup)
        {
            await this.proxyDBService.AddGroupAsync(new DB.Models.DBProxyGroup() { Name = newGroup.First().GroupName });
            return JsonConvert.SerializeObject(true);
        }
    }
}
