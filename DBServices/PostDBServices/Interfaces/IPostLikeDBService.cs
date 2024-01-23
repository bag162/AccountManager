using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostLikeDBService
    {
        public DBPostLikes GetPostLikeById(int id);
        public List<DBPostLikes> GetAllLikes();
        public List<DBPostLikes> GetAllLikesAsNoTracking();
        public Task UpdateLikesAsync(List<DBPostLikes> likes);
        public Task UpdateLikeAsync(DBPostLikes like);
        public Task AddLikesAsync(List<DBPostLikes> likes);
        public Task RemoveLikesAsync(List<DBPostLikes> likes);
    }
}
