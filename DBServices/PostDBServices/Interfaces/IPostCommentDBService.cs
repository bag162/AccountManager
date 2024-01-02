using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostCommentDBService
    {
        public JqueryDataTable<CommentDTO> GetCommentsByPost(int start, int lenght, string searchdata, int postId);
        public Task AddCommentAsync(List<DBPostComment> comments);
        public Task DeleteCommentAsync(List<DBPostComment> comments);
    }
}