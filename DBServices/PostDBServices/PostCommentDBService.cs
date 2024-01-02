using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class PostCommentDBService : IPostCommentDBService
    {
        private AMContext dbcontext;
        private ILogger<PostCommentDBService> logger;
        private IMapper mapper;

        public PostCommentDBService(AMContext amcontext, ILogger<PostCommentDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddCommentAsync(List<DBPostComment> comments)
        {
            await this.dbcontext.PostComment.AddRangeAsync(comments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteCommentAsync(List<DBPostComment> comments)
        {
            this.dbcontext.PostComment.RemoveRange(comments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<CommentDTO> GetCommentsByPost(int start, int lenght, string searchdata, int postId)
        {
            var data = new JqueryDataTable<CommentDTO>();
            data.recordsTotal = this.dbcontext.PostComment.Count();
            DBPostComment[] filteredData = Array.Empty<DBPostComment>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.PostComment.Include(x => x.Account).AsQueryable().Where(m => m.Message.Contains(searchdata)
                                                || m.PostId == postId
                                                || m.Account.Login.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.PostComment.Include(x => x.Account).AsQueryable().Where(m => m.Message.Contains(searchdata)
                                                || m.PostId == postId
                                                || m.Account.Login.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<CommentDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.PostComment.Include(x => x.Account).AsQueryable().Where(x => x.PostId == postId).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.PostComment.Include(x => x.Account).AsQueryable().Where(x => x.PostId == postId).Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }
    }
}
