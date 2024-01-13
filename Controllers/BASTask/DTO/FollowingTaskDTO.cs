using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class GetFollowingTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount InstAccount { get; set; }
        public DBProxy Proxy { get; set; }
        public List<FollowUsefulDataDTO> Follows { get; set; }
        public string TaskType { get; set; }
    }

    public class EndFollowingTaskDTO
    {
        public int WorkerId { get; set; }
    }

    public class IntermediateEndFollowingTaskDTO
    {
        public int WorkerId { get; set; }
        public int FollowId { get; set; }
    }
}
