using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostCommentDBService
    {
        public JqueryDataTable<CommentDTO> GetCommentsByGroup(int start, int lenght, string searchdata, int groupId);
        public JqueryDataTable<CommentDTO> GetComments(int start, int lenght, string searchdata);
        public Task AddCommentAsync(List<DBPostComment> comments);
        public Task AddCommentAsync(List<CRUDCommentDTO> comments);
        public Task DeleteCommentAsync(List<CRUDCommentDTO> comments);
    }
}