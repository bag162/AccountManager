using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.TaskManagers.InstManager;
using Microsoft.AspNetCore.Mvc;

namespace BASAccountManager.Controllers.BASTask
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BASTaskErrorController : ControllerBase
    {
        private IMapper mapper;
        private readonly ILogger<BASTaskErrorController> logger;
        private InstTaskManager InstTaskManager;

        public BASTaskErrorController(IMapper mapper, ILogger<BASTaskErrorController> logger, InstTaskManager InstTaskManager)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.InstTaskManager = InstTaskManager;
        }

        [HttpPost]
        public async Task<string> RegistrationTaskError([FromBody] RegistrationErrorDTO error)
        {
            return await this.InstTaskManager.ErrorRegistrationTaskAsync(error.Error, error.workerId);
        }

        [HttpPost]
        public async Task<string> AuthorizationTaskError([FromBody] AuthorizationErrorDTO error)
        {
            return await this.InstTaskManager.ErrorAuthorizationTaskAsync(error.AuthorizationErrorType, error.workerId);
        }

        [HttpPost]
        public async Task<string> PostingTaskError([FromBody] PostingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorPostingTaskAsync(error.PostingTaskErrorType, error.workerId);
        }

        [HttpPost]
        public async Task<string> PostingIntermediateError([FromBody] PostingIntermediateErrorDTO error)
        {
            return await this.InstTaskManager.ErrorIntermediatePostingAsync(error.WorkerId, error.PostId, error.ErrorMessage);
        }

        [HttpPost]
        public async Task<string> CommentingTaskError([FromBody] CommentingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorCommentingTaskAsync(error.CommentingTaskErrorType, error.workerId);
        }

        [HttpPost]
        public async Task<string> CommentingIntermediateError([FromBody] CommentingIntermediateErrorDTO error)
        {
            return await this.InstTaskManager.ErrorIntermediateCommentingAsync(error.WorkerId, error.PostCommentId, error.ErrorMessage);
        }

        [HttpPost]
        public async Task<string> VerifyServiceError([FromBody] VerifyServiceErrorDTO error)
        {
            return await this.InstTaskManager.ErrorVerifyServiceAsync(error);
        }

        [HttpPost]
        public async Task<string> GlobalError([FromBody] GlobalErrorDTO error)
        {
            return await this.InstTaskManager.ErrorGlobalAsync(error);
        }
    }
}