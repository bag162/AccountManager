using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Facebook.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IFBDBService
    {
        public List<DBFacebookAccount> GetFBAccounts();
        public JqueryDataTable<FBAccountDTO> GetFBAccounts(int start, int lenght, string searchdata);
        public Task AddFBAccountsAsync(List<DBFacebookAccount> newProxy, string accGroup);
        public Task RemoveFBAccountsAsync(List<DBFacebookAccount> removedProxy);
        public Task UpdateFBAccountsAsync(List<DBFacebookAccount> updatedProxy);
        public Task UpdateFBAccountsAsync(List<DBFacebookAccount> updatedProxy, string newGroup);
        public List<string> GetAllGroups();
        public Task AddGroupAsync(DBFBAccountGroup addedGroup);
    }
}
