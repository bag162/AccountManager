using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface ICommentDBService
    {
        public JqueryDataTable<CommentDTO> GetCommentsByGroup(int start, int lenght, string searchdata, int groupId);
        public JqueryDataTable<CommentDTO> GetComments(int start, int lenght, string searchdata);
        public Task AddCommentAsync(List<CRUDCommentDTO> comments);
        public Task RemoveCommentsAsync(List<CRUDCommentDTO> comments);
    }
}
