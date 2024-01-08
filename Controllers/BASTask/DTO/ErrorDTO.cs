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

    public enum PostingTaskErrorType
    {
        FullBan,
        UnrecognizedError,
        DeauthorizedError // TODO impl it in insttaskparser
    }


    // Global Error
    public class GlobalErrorDTO
    {
        public string ExeptionMessage { get; set; }
        public int workerId { get; set; }
    }
}