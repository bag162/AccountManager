using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Facebook.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FBAccountController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private IFBDBService FbDbService { get; set; }
        private readonly ILogger<FBAccountController> logger;

        public FBAccountController(ILogger<FBAccountController> logger, IMapper mapper, IFBDBService FbDbService)
        {
            this.logger  = logger;
            this.mapper = mapper;
            this.FbDbService = FbDbService;
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<FBAccountDTO> returnedData = this.FbDbService.GetFBAccounts(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] FBAccountDTO[] accounts)
        {
            await this.FbDbService.RemoveFBAccountsAsync(mapper.Map<List<DBFacebookAccount>>(accounts.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPost]
        public async Task<string> Post([FromBody] FBAccountDTO[] accounts)
        {
            await this.FbDbService.AddFBAccountsAsync(mapper.Map<List<DBFacebookAccount>>(accounts.ToList()), accounts.First().Group);
            return JsonConvert.SerializeObject("true");
        }

        [HttpPut]
        public async Task<string> Put([FromBody] FBAccountDTO[] accounts)
        {
            await this.FbDbService.UpdateFBAccountsAsync(mapper.Map<List<DBFacebookAccount>>(accounts), accounts.First().Group);
            return JsonConvert.SerializeObject("true");
        }
    }
}