using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

        public List<DBPostComment> GetAllPostComments()
        {
            return this.dbcontext.PostComment.ToList();
        }

        public DBPostComment GetPostCommentById(int id)
        {
            return this.dbcontext.PostComment.Find(id);
        }

        public async Task RemoveCommentsAsync(List<DBPostComment> comments)
        {
            this.dbcontext.PostComment.RemoveRange(comments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdatePostCommentAsync(List<DBPostComment> updateComments)
        {
            this.dbcontext.PostComment.UpdateRange(updateComments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdatePostCommentAsync(DBPostComment updateComments)
        {
            this.dbcontext.Update(updateComments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
