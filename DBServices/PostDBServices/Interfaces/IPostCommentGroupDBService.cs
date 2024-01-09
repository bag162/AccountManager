using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostCommentGroupDBService
    {
        public DBPostCommentGroup GetCommentGroupById(int id);
        public JqueryDataTable<CommentGroupDTO> GetPostCommentGroups(int start, int lenght, string searchdata);
        public Task AddPostCommentGroupAsync(DBPostCommentGroup commentGroup);
        public Task DeleteCommentGroupAsync(List<DBPostCommentGroup> commentGroups);
        public Task<string[]> GetGroupNamesAsync();
    }
}
