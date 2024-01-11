using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostCommentDBService
    {
        public DBPostComment GetPostCommentById(int id);
        public JqueryDataTable<CommentDTO> GetCommentsByGroup(int start, int lenght, string searchdata, int groupId);
        public JqueryDataTable<CommentDTO> GetComments(int start, int lenght, string searchdata);
        public List<DBPostComment> GetAllPostComments();
        public Task UpdatePostCommentAsync(List<DBPostComment> updateComments);
        public Task UpdatePostCommentAsync(DBPostComment updateComments);
        public Task AddCommentAsync(List<DBPostComment> comments);
        public Task AddCommentAsync(List<CRUDCommentDTO> comments);
        public Task DeleteCommentAsync(List<CRUDCommentDTO> comments);
    }
}