using AutoMapper;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class PostLikeDBService : IPostLikeDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public PostLikeDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddLikesAsync(List<DBPostLikes> likes)
        {
            await this.dbcontext.PostLike.AddRangeAsync(likes);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<DBPostLikes> GetAllLikes()
        {
            return this.dbcontext.PostLike.ToList();
        }
        public List<DBPostLikes> GetAllLikesAsNoTracking()
        {
            return this.dbcontext.PostLike.AsNoTracking().ToList();
        }
        public DBPostLikes GetPostLikeById(int id)
        {
            return this.dbcontext.PostLike.Find(id);
        }

        public async Task RemoveLikesAsync(List<DBPostLikes> likes)
        {
            this.dbcontext.PostLike.RemoveRange(likes);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateLikeAsync(DBPostLikes like)
        {
            this.dbcontext.PostLike.Update(like);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateLikesAsync(List<DBPostLikes> likes)
        {
            this.dbcontext.PostLike.UpdateRange(likes);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}