using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BASAccountManager.Controllers.Post.Post
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private readonly ILogger<PostController> logger;
        private IPostDBService postDBService { get; set; }
        public PostController(IMapper mapper, ILogger<PostController> logger, IPostDBService postDBService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.postDBService = postDBService;
        }

        [Route("{clonId:int}")]
        [HttpGet]
        [Authorize(Roles = "/post/manager/update,admin")]
        public string GetByClonId(int start, int length, int draw, int clonId)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<PostListDTO> returnedData = this.postDBService.GetPostByClonId(start, length, searchData.First(), clonId);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [Route("{postId:int}")]
        [HttpGet]
        [Authorize(Roles = "/post/manager/update,admin")]
        public string Get(int postId)
        {
            var post = this.postDBService.GetPostById(postId);
            return JsonConvert.SerializeObject(post);
        }

        [HttpGet]
        [Authorize(Roles = "/post/manager,admin")]
        public async Task<string> Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<PostListDTO> returnedData = this.postDBService.GetPost(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        [Authorize(Roles = "/post/manager/update,admin")]
        public async Task<string> Delete([FromBody] PostListDTO[] group)
        {
            await this.postDBService.DeletePostAsync(mapper.Map<List<DBPost>>(group.ToList()));
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        [Authorize(Roles = "/post/manager/add,admin")]
        public async Task<string> Post([FromBody] CRUDPostDTO post)
        {
            await this.postDBService.AddPostAsync(post);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPut]
        [Authorize(Roles = "/post/manager/update,admin")]
        public async Task<string> Put([FromBody] UpdatePostDTO post)
        {
            await this.postDBService.UpdatePostAsync(post);
            return JsonConvert.SerializeObject(true);
        }
    }
}