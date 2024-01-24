using BASAccountManager.Controllers.Advert.Post.DTO;
using BASAccountManager.Controllers.DTO;

namespace BASAccountManager.DBServices.AdvertDBServices.Interfaces
{
    public interface IAdvertPostGroupDBService
    {
        public Task<string[]> GetAdvertPostGroupNamesAsync();
        public JqueryDataTable<AdvertPostGroupDTO> GetAdvertPostGroups(int start, int lenght, string searchdata);
        public Task AddAdvertPostGroupAsync(AddAdvertPostGroupDTO groups);
        public Task DeleteAdvertPostGroupAsync(List<DeleteAdvertPostGroupDTO> groups);
    }
}