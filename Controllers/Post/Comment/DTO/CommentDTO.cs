using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.Controllers.Post.Comment.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string AccountLogin { get; set; }
        public DateTime? CommentTime { get; set; }
        public string CommentStatus { get; set; }
    }
}
