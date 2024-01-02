using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

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

        [Route("{postId:int}")]
        [HttpGet]
        public string Get(int postId)
        {
            var post = this.postDBService.GetPostById(postId);
            post.ImagePath = post.ImagePath.Remove(0, 8);
            return JsonConvert.SerializeObject(post);
        }

        [HttpGet]
        public async Task<string> Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<PostListDTO> returnedData = this.postDBService.GetPost(start, length, searchData.First());
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] PostListDTO[] group)
        {
            await this.postDBService.DeletePostAsync(mapper.Map<List<DBPost>>(group.ToList()));
            return JsonConvert.SerializeObject(true);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] CRUDPostDTO post)
        {
            await this.postDBService.AddPostAsync(post);
            return JsonConvert.SerializeObject(true);
        }

        [HttpPut]
        public async Task<string> Put([FromBody] UpdatePostDTO post)
        {
            await this.postDBService.UpdatePostAsync(post);
            return JsonConvert.SerializeObject(true);
        }
    }
}