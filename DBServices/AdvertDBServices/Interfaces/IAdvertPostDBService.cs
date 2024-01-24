using BASAccountManager.Controllers.Advert.Post.DTO;
using BASAccountManager.Controllers.DTO;

namespace BASAccountManager.DBServices.AdvertDBServices.Interfaces
{
    public interface IAdvertPostDBService
    {
        public JqueryDataTable<AdvertPostDTO> GetAdvertPostByGroup(int start, int lenght, string searchdata, int groupId);
        public JqueryDataTable<AdvertPostDTO> GetAdvertPost(int start, int lenght, string searchdata);
        public Task AddAdvertPostAsync(List<AddAdvertPostDTO> accounts);
        public Task DeleteAdvertPostAsync(List<int> ids);
    }
}