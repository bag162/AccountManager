using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.Advert.Post.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Advert.Post
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdvertPostGroupController : ControllerBase
    {
        private IAdvertPostGroupDBService advertPostGroupDBService { get; set; }
        public AdvertPostGroupController(IAdvertPostGroupDBService advertPostGroupDBService)
        {
            this.advertPostGroupDBService = advertPostGroupDBService;
        }

        [HttpGet]
        public async Task<string> Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<AdvertPostGroupDTO> returnedData = this.advertPostGroupDBService.GetAdvertPostGroups(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpGet]
        public async Task<string> GetList()
        {
            return JsonConvert.SerializeObject(await this.advertPostGroupDBService.GetAdvertPostGroupNamesAsync());
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] DeleteAdvertPostGroupDTO[] groups)
        {
            await this.advertPostGroupDBService.DeleteAdvertPostGroupAsync(groups.ToList());
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] AddAdvertPostGroupDTO group)
        {
            await this.advertPostGroupDBService.AddAdvertPostGroupAsync(group);
            return JsonConvert.SerializeObject(true);
        }
    }
}