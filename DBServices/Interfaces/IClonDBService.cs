using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IClonDBService
    {
        public List<DBClon> GetClones();
        public DBClon GetClonById(int id);
        public Task<int> AddCloneAsync(DBClon data);
        public Task UpdateCloneAsync(DBClon data);
        public Task DeleteClonesAsync(List<DBClon> data);
    }
}
