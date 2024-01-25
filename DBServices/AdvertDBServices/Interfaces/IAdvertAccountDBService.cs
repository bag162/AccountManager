using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.ProfileFilling.DTO;
using BASAccountManager.DB.Models.AdvertResourses;

namespace BASAccountManager.DBServices.AdvertDBServices.Interfaces
{
    public interface IAdvertAccountDBService
    {
        public JqueryDataTable<AdvertAccountDTO> GetAdvertAccountByGroup(int start, int lenght, string searchdata, int groupId);
        public JqueryDataTable<AdvertAccountDTO> GetAdvertAccount(int start, int lenght, string searchdata);
        public Task<List<DBAdvertAccount>> GetAdvertAccountsByGroupAsync(string groupName);
        public List<DBAdvertAccount> GetAdvertAccounts();
        public DBAdvertAccount GetAdvertAccountById(int id);

        public Task UpdateAvertAccountsAsync(List<DBAdvertAccount> accounts);

        public Task AddAdvertAccountAsync(List<AddAdvertAccountDTO> accounts);
        public Task DeleteAdvertAccountAsync(List<int> ids);
    }
}