using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.Task.DTO
{
    public class RegistrationTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public int CountAccount { get; set; }

        public RegistrationVerifyResoursesType ResoursesType { get; set; }
        public int? SMSServiceId { get; set; }
        public int? EmailServiceId { get; set; }
    }

    public class AuthorizationTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
    }

    public class PostingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public int PostPerAccount { get; set; }
    }
}