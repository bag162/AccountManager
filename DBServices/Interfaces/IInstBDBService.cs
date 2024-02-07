using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Instagram.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IInstDBService
    {
        public Task<List<DBInstagramAccount>> GetInstAccountsByGroupAsync(string group);
        public List<DBInstagramAccount> GetInstAccounts();
        public JqueryDataTable<InstAccountDTO> GetInstAccounts(int start, int lenght, string searchdata);
        public Task AddInstAccountsAsync(List<DBInstagramAccount> newAccount, string accGroup);
        public Task<int> AddInstAccountsAsync(DBInstagramAccount newAccount, string accGroup);
        public Task RemoveInstAccountsAsync(List<DBInstagramAccount> removedAccount);
        public Task UpdateInstAccountsAsync(List<DBInstagramAccount> updatedAccount);
        public Task UpdateInstAccountsAsync(List<DBInstagramAccount> updatedAccount, string newGroup);
        public List<string> GetAllGroups();
        public Task<int> AddGroupAsync(DBInstAccountGroup addedGroup);
    }
}
