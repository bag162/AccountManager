using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.Advert.Post.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Advert.Post
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "/advertresourses/post/manager,admin")]
    public class AdvertPostController : ControllerBase
    {
        private IAdvertPostDBService advertPostDBService { get; set; }
        public AdvertPostController(IAdvertPostDBService advertPostDBService)
        {
            this.advertPostDBService = advertPostDBService;
        }

        [Route("{groupId:int}")]
        [HttpGet]
        public string Get(int start, int length, int draw, int groupId)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<AdvertPostDTO> returnedData = this.advertPostDBService.GetAdvertPostByGroup(start, length, searchData.First(), groupId);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpGet]
        public async Task<string> Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<AdvertPostDTO> returnedData = this.advertPostDBService.GetAdvertPost(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] DeleteAdvertPostDTO[] posts)
        {
            await this.advertPostDBService.DeleteAdvertPostAsync(posts.Select(x => x.Id).ToList());
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] AddAdvertPostDTO[] posts)
        {
            await this.advertPostDBService.AddAdvertPostAsync(posts.ToList());
            return JsonConvert.SerializeObject(true);
        }
    }
}