using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class GetLikingTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount InstAccount { get; set; }
        public DBProxy Proxy { get; set; }
        public List<LikeUsefulDataDTO> Likes { get; set; }
        public string TaskType { get; set; }
    }

    public class EndLikingTaskDTO
    {
        public int WorkerId { get; set; }
    }

    public class IntermediateEndLikingTaskDTO
    {
        public int WorkerId { get; set; }
        public int PostLikeId { get; set; }
    }
}