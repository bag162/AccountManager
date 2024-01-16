using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Post.Comment
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "/post/comment/manager,admin")]
    public class PostCommentController : ControllerBase
    {
        private ICommentDBService commentDBService;
        private IMapper mapper { get; set; }
        private readonly ILogger<PostCommentController> logger;

        public PostCommentController(IMapper mapper, ILogger<PostCommentController> logger, ICommentDBService commentDBService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.commentDBService = commentDBService;
        }

        [Route("{groupId:int}")]
        [HttpGet]
        public string Get(int start, int length, int draw, int groupId)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<CommentDTO> returnedData = this.commentDBService.GetCommentsByGroup(start, length, searchData, groupId);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<CommentDTO> returnedData = this.commentDBService.GetComments(start, length, searchData);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpPost]
        public async Task<string> Post([FromBody] CRUDCommentDTO[] comments)
        {
            await this.commentDBService.AddCommentAsync(comments.ToList());
            return JsonConvert.SerializeObject(true);
        }

        [HttpDelete]
        public async Task<string> Delete([FromBody] CRUDCommentDTO[] comments)
        {
            await this.commentDBService.RemoveCommentsAsync(comments.ToList());
            return JsonConvert.SerializeObject(true);
        }
    }
}
