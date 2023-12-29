using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class GetAuthorizationTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount InstAccount { get; set; }
        public DBProxy Proxy { get; set; }
        public string TaskType { get; set; }
    }

    public class EndAuthorizationTaskDTO
    {
        public int TaskWorkerId { get; set; }
        public string ProfileLink { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}