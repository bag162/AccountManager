using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.Task.DTO
{
    public class WorkerTaskDTO
    {
        public int Id { get; set; }
        public int? WorkerId { get; set; }
        public string? InstanceId { get; set; }
        public string Status { get; set; }
        public string InstAccountLogin { get; set; }
        public string ProxyId { get; set; }
        public string? UsefulData { get; set; }
        public string? ErrorMessage { get; set; }
    }
}