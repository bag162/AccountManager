using BASAccountManager.Controllers.Clon.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IClonDBService
    {
        public ClonData GetClonData(int clonId);
        public JqueryDataTable<GetClonDTO> GetClones(int start, int lenght, string searchdata);
        public JqueryDataTable<GetClonDTO> GetClonesByGroup(int start, int lenght, string searchdata, int groupId);
        public List<DBClon> GetClones();
        public DBClon GetClonById(int id);
        public Task<int> AddCloneAsync(DBClon data);
        public Task UpdateCloneAsync(DBClon data);
        public Task DeleteClonesAsync(List<DBClon> data);
        public Task DelteteClonesByIdAsync(int[] ids);
    }
}
