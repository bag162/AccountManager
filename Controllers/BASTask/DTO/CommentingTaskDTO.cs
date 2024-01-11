using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class GetCommentingTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount InstAccount { get; set; }
        public DBProxy Proxy { get; set; }
        public List<CommentUsefulDataDTO> Comments { get; set; }
        public string TaskType { get; set; }
    }

    public class EndCommentingTaskDTO
    {
        public int WorkerId { get; set; }
    }

    public class IntermediateEndCommentingTaskDTO
    {
        public int WorkerId { get; set; }
        public int PostCommentId { get; set; }
    }

}