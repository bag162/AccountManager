using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.Controllers.Post.Comment.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string CommentGroupName{ get; set; }
    }

    public class CRUDCommentDTO
    {
        public int? Id { get; set; }
        public string? Message { get; set; }
        public string? CommentGroupName { get; set; }
    }
}