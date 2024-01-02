using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace BASAccountManager.Controllers.Post.Comment
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostCommentController : ControllerBase
    {
        private IMapper mapper { get; set; }
        private readonly ILogger<PostCommentController> logger;

        public PostCommentController(IMapper mapper, ILogger<PostCommentController> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}
