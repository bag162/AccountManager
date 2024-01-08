using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class GetPostingTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount InstAccount { get; set; }
        public DBProxy Proxy { get; set; }
        public List<DBPost> Posts { get; set; }
        public string TaskType { get; set; }
    }

    public class EndPostingTaskDTO
    {
        public int workerId { get; set; }
    }

    public class IntermediateEndPostingTaskDTO
    {
        public int workerId { get; set; }
        public int postId { get; set; }
        public string postURI { get; set; }
    }
}