using BASAccountManager.DB.Models;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Task.DTO
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string ClientTaskName { get; set; }
        public string Status { get; set; }
        public string TaskType { get; set; }
        public string UsefulData { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
    }

    public class RegistrationTaskWorkerUsefulDataDTO
    {
        public int CountAccount { get; set; }

        public RegistrationVerifyResoursesType RegistrationVerifyResoursesType { get; set; }
        public int? SMSServiceId { get; set; }
        public int? EmailServiceId { get; set; }
    }
}