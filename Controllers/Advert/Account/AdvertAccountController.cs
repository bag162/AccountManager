using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BASAccountManager.Controllers.Advert.Account
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdvertAccountController : ControllerBase
    {
        private IAdvertAccountDBService advertAccountDBService { get; set; }

        public AdvertAccountController(IAdvertAccountDBService advertAccountDBService)
        {
            this.advertAccountDBService = advertAccountDBService;
        }

        [Route("{groupId:int}")]
        [HttpGet]
        public string Get(int start, int length, int draw, int groupId)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<AdvertAccountDTO> returnedData = this.advertAccountDBService.GetAdvertAccountByGroup(start, length, searchData.First(), groupId);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpGet]
        public async Task<string> Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<AdvertAccountDTO> returnedData = this.advertAccountDBService.GetAdvertAccount(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] DeleteAdvertAccountDTO[] accounts)
        {
            await this.advertAccountDBService.DeleteAdvertAccountAsync(accounts.Select(x => x.Id).ToList());
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] AddAdvertAccountDTO[] account)
        {
            await this.advertAccountDBService.AddAdvertAccountAsync(account.ToList());
            return JsonConvert.SerializeObject(true);
        }
    }
}