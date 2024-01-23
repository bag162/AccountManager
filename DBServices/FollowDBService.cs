using AutoMapper;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices
{
    public class FollowDBService : IFollowDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public FollowDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddFollowsAsync(List<DBFollow> follows)
        {
            await this.dbcontext.Follow.AddRangeAsync(follows);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public DBFollow GetFollowById(int id)
        {
            return this.dbcontext.Follow.Include(x => x.RecipientAccount).Include(x => x.SenderAccount).Where(x => x.Id == id).First();
        }

        public List<DBFollow> GetFollows()
        {
            return this.dbcontext.Follow.ToList();
        }

        public List<DBFollow> GetFollowsByRecipientAccountId(int id)
        {
            return this.dbcontext.Follow.Where(x => x.RecipientAccountId == id).Include(x => x.SenderAccount).Include(x => x.RecipientAccount).ToList();
        }

        public List<DBFollow> GetFollowsBySenderAccountId(int id)
        {
            return this.dbcontext.Follow.Where(x => x.SenderAccountId == id).Include(x => x.SenderAccount).Include(x => x.RecipientAccount).ToList();
        }

        public async Task RemoveByIdAsync(int id)
        {
            var followToRemove = await this.dbcontext.Follow.FindAsync(id);
            this.dbcontext.Follow.Remove(followToRemove);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task RemoveByIdsAsync(List<int> ids)
        {
            var followToRemove = new List<DBFollow>();
            foreach (var followId in ids)
            {
                followToRemove.Add(await this.dbcontext.Follow.FindAsync(followId));
            }
            this.dbcontext.RemoveRange(followToRemove);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task RemoveFollows(List<DBFollow> follows)
        {
            this.dbcontext.Follow.RemoveRange(follows);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateFollowAsync(DBFollow follow)
        {
            this.dbcontext.Follow.Update(follow);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateFollowsAsync(List<DBFollow> follows)
        {
            this.dbcontext.Follow.UpdateRange(follows);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
