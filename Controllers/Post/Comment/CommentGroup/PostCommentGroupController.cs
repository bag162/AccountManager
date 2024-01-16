using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace BASAccountManager.Controllers.Post.Comment.CommentGroup
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin")]
    [Authorize(Roles = "/post/comment/group")]
    public class PostCommentGroupController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private readonly ILogger<PostCommentGroupController> logger;
        public IPostCommentGroupDBService PostCommentGroupDBService { get; set; }

        public PostCommentGroupController(IMapper mapper, ILogger<PostCommentGroupController> logger, IPostCommentGroupDBService PostCommentGroupDBService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.PostCommentGroupDBService = PostCommentGroupDBService;
        }

        [HttpGet]
        public string Get(int start, int length, int draw)
        {
            StringValues searchData;
            this.Request.Query.TryGetValue("search[value]", out searchData);
            JqueryDataTable<CommentGroupDTO> returnedData = this.PostCommentGroupDBService.GetPostCommentGroups(start, length, searchData);
            returnedData.draw = draw;
            return JsonConvert.SerializeObject(returnedData);
        }

        [HttpPost]
        public async Task<string> Post(CRUDCommentGroupDTO newGroup)
        {
            await this.PostCommentGroupDBService.AddPostCommentGroupAsync(new DB.Models.Post.DBPostCommentGroup() { Name = newGroup.Name });
            return JsonConvert.SerializeObject(true);
        }

        [HttpDelete]
        public async Task<string> Delete(CRUDCommentGroupDTO[] deletedGroups)
        {
            await this.PostCommentGroupDBService.DeleteCommentGroupAsync(this.mapper.Map<List<DBPostCommentGroup>>(deletedGroups.ToList()));
            return JsonConvert.SerializeObject(true);
        }

        [HttpGet]
        public async Task<string> GetGroupNames()
        {
            return JsonConvert.SerializeObject(await this.PostCommentGroupDBService.GetGroupNamesAsync());
        }
    }
}