using AutoMapper;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}