using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IFollowDBService
    {
        public List<DBFollow> GetFollows();
        public DBFollow GetFollowById(int id);
        public List<DBFollow> GetFollowsByRecipientAccountId(int id);
        public List<DBFollow> GetFollowsBySenderAccountId(int id);
        public Task AddFollowsAsync(List<DBFollow> follows);
        public Task UpdateFollowsAsync(List<DBFollow> follows);
        public Task UpdateFollowAsync(DBFollow follow);
        public Task RemoveByIdAsync(int id);
        public Task RemoveByIdsAsync(List<int> ids);
        public Task RemoveFollows(List<DBFollow> follows);
    }
}