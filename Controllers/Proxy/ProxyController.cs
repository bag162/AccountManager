using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Proxy
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "/proxy,admin")]
    public class ProxyController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IProxyDBService proxyDBService { get; set; }
        private readonly ILogger<ProxyController> logger;

        public ProxyController(ILogger<ProxyController> logger, IProxyDBService proxyDBService, IMapper mapper)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.proxyDBService = proxyDBService;
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<ProxyDTO> returnedData = this.proxyDBService.GetProxy(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] ProxyDTO[] proxy)
        {
            await this.proxyDBService.RemoveProxyAsync(mapper.Map<List<DBProxy>>(proxy.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPost]
        public async Task<string> Post([FromBody] ProxyDTO[] proxy)
        {
            await this.proxyDBService.AddProxyAsync(mapper.Map<List<DBProxy>>(proxy.ToList()), proxy.First().Group);
            return JsonConvert.SerializeObject("true");
        }

        [HttpPut]
        public async Task<string> Put([FromBody] ProxyDTO[] proxy)
        {
            await this.proxyDBService.UpdateProxyAsync(mapper.Map<List<DBProxy>>(proxy), proxy.First().Group);
            return JsonConvert.SerializeObject("true");
        }
    }
}