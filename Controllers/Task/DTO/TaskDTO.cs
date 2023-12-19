using BASAccountManager.DB.Models;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Task.DTO
{
    public class TaskDTO
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string TaskType { get; set; }
        public string UsefulData { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
    }

    public class RegistrationTaskWorkerUsefulDataDTO
    {
        public int SMSServiceId { get; set; }
        public int CountAccount { get; set; }
    }
}