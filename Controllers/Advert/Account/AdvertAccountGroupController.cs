using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Advert.Account
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdvertAccountGroupController : ControllerBase
    {
        private IAdvertAccountGroupDBService advertAccountGroupDBService { get; set; }
        public AdvertAccountGroupController(IAdvertAccountGroupDBService advertAccountGroupDBService)
        {
            this.advertAccountGroupDBService = advertAccountGroupDBService;
        }

        [HttpGet]
        public async Task<string> Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<AdvertAccountGroupDTO> returnedData = this.advertAccountGroupDBService.GetAdvertAccountGroups(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpGet]
        public async Task<string> GetList()
        {
            return JsonConvert.SerializeObject(await this.advertAccountGroupDBService.GetAdvertAccountGroupNamesAsync());
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] DeleteAdvertAccountGroupDTO[] groups)
        {
            await this.advertAccountGroupDBService.DeleteAdvertAccountGroupAsync(groups.ToList());
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] AddAdvertAccountGroupDTO group)
        {
            await this.advertAccountGroupDBService.AddAdvertAccountGroupAsync(group);
            return JsonConvert.SerializeObject(true);
        }
    }
}