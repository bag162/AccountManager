using AutoMapper;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BASAccountManager.DBServices
{
    public class InstPostDBService : IInstPostDBService
    {
        private AMContext dbcontext;
        private ILogger<InstPostDBService> logger;
        private IMapper mapper;

        public InstPostDBService(AMContext dbcontext, ILogger<InstPostDBService> logger, IMapper mapper)
        {
            this.dbcontext = dbcontext;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddInstPostAsync(DBInstPost post)
        {
            await this.dbcontext.InstPost.AddAsync(post);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task<List<DBInstPost>> GetAllInstPostAsync()
        {
            return await this.dbcontext.InstPost
                .Include(x => x.Post).ThenInclude(x => x.PostCommentGroup).ThenInclude(x => x.ListComment)
                .Include(x => x.ListComment).ThenInclude(x => x.Comment)
                .ToListAsync();
        }

        public async Task<List<DBInstPost>> GetAllInstPostAsyncAsNoTracking()
        {
            return await this.dbcontext.InstPost
                 .Include(x => x.Post).ThenInclude(x => x.PostCommentGroup).ThenInclude(x => x.ListComment)
                 .Include(x => x.ListComment).ThenInclude(x => x.Comment)
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<List<DBInstPost>> GetAllPostByGroupAsync(string group)
        {
            var postgroupId = this.dbcontext.PostGroup.Where(x => x.Name == group).Select(x => x.Id).First();
            var instPosts = this.dbcontext.InstPost.Include(x => x.ListComment).ThenInclude(x => x.Comment).Include(x => x.Post).Where(x => x.Post.GroupId == postgroupId).ToList();
            return instPosts;
        }

        public List<DBInstPost> GetInstPospsByAccount(int accountId)
        {
            return this.dbcontext.InstPost.Where(x => x.AccountId == accountId).Include(x => x.Post).ToList();
        }

        public async Task UpdateInstPostAsync(DBInstPost post)
        {
            this.dbcontext.InstPost.Update(post);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateInstPostAsync(List<DBInstPost> posts)
        {
            this.dbcontext.InstPost.UpdateRange(posts);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}