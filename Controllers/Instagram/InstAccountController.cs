using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Instagram.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "/Instaccount,admin")]
    public class InstAccountController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IInstDBService InstDbService { get; set; }
        private readonly ILogger<InstAccountController> logger;

        public InstAccountController(ILogger<InstAccountController> logger, IMapper mapper, IInstDBService InstDbService)
        {
            this.logger  = logger;
            this.mapper = mapper;
            this.InstDbService = InstDbService;
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<InstAccountDTO> returnedData = this.InstDbService.GetInstAccounts(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] InstAccountDTO[] accounts)
        {
            await this.InstDbService.RemoveInstAccountsAsync(mapper.Map<List<DBInstagramAccount>>(accounts.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPost]
        public async Task<string> Post([FromBody] InstAccountDTO[] accounts)
        {
            var newAccounts = mapper.Map<List<DBInstagramAccount>>(accounts.ToList());
            foreach (var acc in newAccounts)
            {
                acc.AccountStatus = AccountStatus.NotAuthorized;
            }
            await this.InstDbService.AddInstAccountsAsync(newAccounts, accounts.First().Group);
            return JsonConvert.SerializeObject("true");
        }

        [HttpPut]
        public async Task<string> Put([FromBody] InstAccountDTO[] accounts)
        {
            await this.InstDbService.UpdateInstAccountsAsync(mapper.Map<List<DBInstagramAccount>>(accounts), accounts.First().Group);
            return JsonConvert.SerializeObject("true");
        }
    }
}