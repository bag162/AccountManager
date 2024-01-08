using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Group.DTO;
using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostGroupDBService
    {
        public List<string> GetListGroupNames();
        public List<DBPostGroup> GetGroups();
        public JqueryDataTable<PostGroupDTO> GetGroups(int start, int lenght, string searchdata);
        public Task AddGroupsAsync(List<PostGroupDTO> groups);
        public Task DeleteGroupsAsync(List<DBPostGroup> groups);
    }
}