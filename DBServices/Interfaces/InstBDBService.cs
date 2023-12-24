using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Instagram.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IInstDBService
    {
        public List<DBInstagramAccount> GetInstAccounts();
        public JqueryDataTable<InstAccountDTO> GetInstAccounts(int start, int lenght, string searchdata);
        public Task AddInstAccountsAsync(List<DBInstagramAccount> newAccount, string accGroup);
        public Task<int> AddInstAccountsAsync(DBInstagramAccount newAccount, string accGroup);
        public Task RemoveInstAccountsAsync(List<DBInstagramAccount> removedAccount);
        public Task UpdateInstAccountsAsync(List<DBInstagramAccount> updatedAccount);
        public Task UpdateInstAccountsAsync(List<DBInstagramAccount> updatedAccount, string newGroup);
        public List<string> GetAllGroups();
        public Task AddGroupAsync(DBInstAccountGroup addedGroup);
    }
}
