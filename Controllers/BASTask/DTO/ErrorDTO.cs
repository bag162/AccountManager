namespace BASAccountManager.Controllers.BASTask.DTO
{

    // Registration Error
    public class RegistrationErrorDTO
    {
        public RegistrationTaskErrorType Error { get; set; }
        public int workerId { get; set; }
    }

    public enum RegistrationTaskErrorType
    {
        FullBan,
        InputCodeCheckpointBan,
        OtherError
    }

    // Verify data Error
    public class VerifyServiceErrorDTO
    {
        public VerifyServiceErrorType VerifyServiceTypeError { get; set; }
        public int workerId { get; set; }
    }

    public enum VerifyServiceErrorType
    {
        LowBalance
    }

    // Authorization Error
    public class AuthorizationErrorDTO
    {
        public AuthorizationTaskErrorType AuthorizationErrorType { get; set; }
        public int workerId { get; set; }
    }

    public enum AuthorizationTaskErrorType
    {
        FullBan,
        IncorrectAuthData,
        UnrecognizedError
    }

    public class PostingErrorDTO
    {
        public PostingTaskErrorType PostingTaskErrorType { get; set; }
        public int workerId { get; set; }
    }

    public class PostingIntermediateErrorDTO
    {
        public int WorkerId { get; set; }
        public int PostId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum PostingTaskErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }

    public class CommentingErrorDTO
    {
        public CommentingTaskErrorType CommentingTaskErrorType { get; set; }
        public int workerId { get; set; }
    }

    public class CommentingIntermediateErrorDTO
    {
        public int WorkerId { get; set; }
        public int PostCommentId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum CommentingTaskErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }

    public class LikingErrorDTO
    {
        public LikingTaskErrorType LikingTaskErrorType { get; set; }
        public int workerId { get; set; }
    }

    public class LikingIntermediateErrorDTO
    {
        public int WorkerId { get; set; }
        public int PostLikeId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum LikingTaskErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }

    public class FollowingErrorDTO
    {
        public FollowingTaskErrorType FollowingTaskErrorType { get; set; }
        public int workerId { get; set; }
    }

    public class FollowingIntermediateErrorDTO
    {
        public int WorkerId { get; set; }
        public int FollowId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum FollowingTaskErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }

    public class ProfileFillingErrorDTO
    {
        public ProfileFillingTaskErrorType ProfileFillingTaskErrorType { get; set; }
        public int workerId { get; set; }
    }

    public enum ProfileFillingTaskErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }

    public class AdvertFollowingIntermediateErrorDTO
    {
        public int WorkerId { get; set; }
        public int AdvertAccountId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AdvertFollowingErrorDTO
    {
        public int WorkerId { get; set; }
        public AdvertFollowingErrorType AdvertFollowingErrorType { get; set; }
    }

    public enum AdvertFollowingErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }

    public class AdvertLikingIntermediateErrorDTO
    {
        public int WorkerId { get; set; }
        public int AdvertPostId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AdvertLikingErrorDTO
    {
        public int WorkerId { get; set; }
        public AdvertLikingErrorType AdvertFollowingErrorType { get; set; }
    }

    public enum AdvertLikingErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }

    public class AdvertCommentingIntermediateErrorDTO
    {
        public int WorkerId { get; set; }
        public int AdvertPostId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AdvertCommentingErrorDTO
    {
        public int WorkerId { get; set; }
        public AdvertCommentingErrorType AdvertFollowingErrorType { get; set; }
    }

    public enum AdvertCommentingErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError
    }


    // Global Error
    public class GlobalErrorDTO
    {
        public string ExeptionMessage { get; set; }
        public int workerId { get; set; }
    }
}