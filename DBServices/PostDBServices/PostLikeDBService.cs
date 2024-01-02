using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Like.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class PostLikeDBService : IPostLikeDBService
    {
        private AMContext dbcontext;
        private ILogger<PostLikeDBService> logger;
        private IMapper mapper;

        public PostLikeDBService(AMContext amcontext, ILogger<PostLikeDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddLikeAsync(List<DBPostLike> likes)
        {
            await this.dbcontext.PostLike.AddRangeAsync(likes);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteLikeAsync(List<DBPostLike> likes)
        {
            this.dbcontext.PostLike.RemoveRange(likes);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<LikeDTO> GetLikeByPost(int start, int lenght, string searchdata, int postId)
        {
            var data = new JqueryDataTable<LikeDTO>();
            data.recordsTotal = this.dbcontext.PostLike.Count();
            DBPostLike[] filteredData = Array.Empty<DBPostLike>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.PostLike.Include(x => x.Account).AsQueryable().Where(m => m.Account.Login.Contains(searchdata)
                                                || m.PostId == postId
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.PostLike.Include(x => x.Account).AsQueryable().Where(m => m.Account.Login.Contains(searchdata)
                                                || m.PostId == postId
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<LikeDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<LikeDTO>>(this.dbcontext.PostLike.Include(x => x.Account).AsQueryable().Where(x => x.PostId == postId).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<LikeDTO>>(this.dbcontext.PostLike.Include(x => x.Account).AsQueryable().Where(x => x.PostId == postId).Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }
    }
}
