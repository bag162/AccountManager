using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.DTO;

namespace BASAccountManager.DBServices.AdvertDBServices.Interfaces
{
    public interface IAdvertAccountGroupDBService
    {
        public Task<string[]> GetAdvertAccountGroupNamesAsync();
        public JqueryDataTable<AdvertAccountGroupDTO> GetAdvertAccountGroups(int start, int lenght, string searchdata);
        public Task AddAdvertAccountGroupAsync(AddAdvertAccountGroupDTO groups);
        public Task DeleteAdvertAccountGroupAsync(List<DeleteAdvertAccountGroupDTO> groups);
    }
}