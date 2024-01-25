using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.TaskManagers.InstManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;

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
        public async Task<string> LikingTaskError([FromBody] LikingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorLikingTaskAsync(error.LikingTaskErrorType, error.workerId);
        }
        
        [HttpPost]
        public async Task<string> LikingIntermediateError([FromBody] LikingIntermediateErrorDTO error)
        {
            return await this.InstTaskManager.ErrorIntermediateLikingAsync(error.WorkerId, error.PostLikeId, error.ErrorMessage);
        }

        [HttpPost]
        public async Task<string> FollowingTaskError([FromBody] FollowingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorFollowingTaskAsync(error.FollowingTaskErrorType, error.workerId);
        }

        [HttpPost]
        public async Task<string> FollowingIntermediateError([FromBody] FollowingIntermediateErrorDTO error)
        {
            return await this.InstTaskManager.ErrorIntermediateFollowingAsync(error.WorkerId, error.FollowId, error.ErrorMessage);
        }

        [HttpPost]
        public async Task<string> FillingProfileError([FromBody] ProfileFillingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorProfileFillingTaskAsync(error.ProfileFillingTaskErrorType, error.workerId);
        }

        [HttpPost]
        public async Task<string> AdvertFollowingIntermediateError([FromBody] AdvertFollowingIntermediateErrorDTO error)
        {
            return await this.InstTaskManager.ErrorIntermediateAdvertFollowingAsync(error.WorkerId, error.AdvertAccountId, error.ErrorMessage);
        }

        [HttpPost]
        public async Task<string> AdvertFollowingError([FromBody] AdvertFollowingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorAdvertFollowingAsync(error.AdvertFollowingErrorType, error.WorkerId);
        }

        [HttpPost]
        public async Task<string> AdvertLikingIntermediateError([FromBody] AdvertLikingIntermediateErrorDTO error)
        {
            return await this.InstTaskManager.ErrorIntermediateAdvertLikingAsync(error.WorkerId, error.AdvertPostId, error.ErrorMessage);
        }

        [HttpPost]
        public async Task<string> AdvertLikingError([FromBody] AdvertLikingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorAdvertLikingAsync(error.AdvertFollowingErrorType, error.WorkerId);
        }

        [HttpPost]
        public async Task<string> AdvertCommentingIntermediateError([FromBody] AdvertCommentingIntermediateErrorDTO error)
        {
            return await this.InstTaskManager.ErrorIntermediateAdvertCommentingAsync(error.WorkerId, error.AdvertPostId, error.ErrorMessage);
        }

        [HttpPost]
        public async Task<string> AdvertCommentingError([FromBody] AdvertCommentingErrorDTO error)
        {
            return await this.InstTaskManager.ErrorAdvertCommentingAsync(error.AdvertFollowingErrorType, error.WorkerId);
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