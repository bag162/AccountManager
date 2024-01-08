using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IInstPostDBService
    {
        public Task AddInstPostAsync(DBInstPost post);
        public List<DBInstPost> GetInstPospsByAccount(int accountId);
        public Task UpdateInstPostAsync(DBInstPost post);
    }
}
