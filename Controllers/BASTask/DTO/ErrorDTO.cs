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

    // Global Error
    public class GlobalErrorDTO
    {
        public string ExeptionMessage { get; set; }
        public int workerId { get; set; }
    }
}