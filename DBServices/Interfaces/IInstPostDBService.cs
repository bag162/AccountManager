using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IInstPostDBService
    {
        public Task AddInstPostAsync(DBInstPost post);
        public Task<List<DBInstPost>> GetAllInstPostAsyncAsNoTracking();
        public Task<List<DBInstPost>> GetAllInstPostAsync();
        public Task<List<DBInstPost>> GetAllPostByGroupAsync(string group);
        public List<DBInstPost> GetInstPospsByAccount(int accountId);

        public Task UpdateInstPostAsync(DBInstPost post);
        public Task UpdateInstPostAsync(List<DBInstPost> posts);
    }
}
