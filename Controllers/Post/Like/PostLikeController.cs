using AutoMapper;
using BASAccountManager.Controllers.Post.Comment;
using Microsoft.AspNetCore.Mvc;

namespace BASAccountManager.Controllers.Post.Like
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostLikeController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private readonly ILogger<PostLikeController> logger;

        public PostLikeController(IMapper mapper, ILogger<PostLikeController> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}
