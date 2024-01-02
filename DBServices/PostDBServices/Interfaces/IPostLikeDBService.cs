using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Like.DTO;
using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostLikeDBService
    {
        public JqueryDataTable<LikeDTO> GetLikeByPost(int start, int lenght, string searchdata, int postId);
        public Task AddLikeAsync(List<DBPostLike> likes);
        public Task DeleteLikeAsync(List<DBPostLike> likes);
    }
}