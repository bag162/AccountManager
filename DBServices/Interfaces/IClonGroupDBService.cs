using BASAccountManager.Controllers.Clon.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IClonGroupDBService
    {
        public JqueryDataTable<GetClonGroupDTO> GetGroups(int start, int lenght, string searchdata);
        public Task<List<DBClonGroup>> GetGroups();
        public DBClonGroup GetGroupByName(string name);
        public List<string> GetGroupNames();
        public Task<int> AddClonGroupAsync(AddClonGroupDTO data);
    }
}
