using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Group.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Post.Group
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "/post/group,admin")]
    public class PostGroupController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private readonly ILogger<PostGroupController> logger;
        private IPostGroupDBService postGroupDBService { get; set; }
        private IPostDBService postDBService { get; set; }

        public PostGroupController(IMapper mapper, 
            ILogger<PostGroupController> logger, 
            IPostGroupDBService postGroupDBService,
            IPostDBService postDBService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.postGroupDBService = postGroupDBService;
            this.postDBService = postDBService;
        }

        [HttpGet]
        public string GetList()
        {
            return JsonConvert.SerializeObject(this.postGroupDBService.GetListGroupNames());
        }

        [Route("{groupId:int}")]
        [HttpGet]
        public string Get(int start, int length, int draw, int groupId)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<PostListDTO> returnedData = this.postDBService.GetPostByPostGroup(start, length, searchData.First(), groupId);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<PostGroupDTO> returnedData = this.postGroupDBService.GetGroups(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] PostGroupDTO[] group)
        {
            await this.postGroupDBService.DeleteGroupsAsync(mapper.Map<List<DBPostGroup>>(group.ToList()));
            return JsonConvert.SerializeObject("true");
        }

        [HttpPost]
        public async Task<string> Post([FromBody] PostGroupDTO[] group)
        {
            await this.postGroupDBService.AddGroupsAsync(group.ToList());
            return JsonConvert.SerializeObject("true");
        }
    }
}