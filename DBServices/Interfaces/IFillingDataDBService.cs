using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.ProfileFilling.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IFillingDataDBService
    {
        public JqueryDataTable<ProfileFillingTableDTO> GetFillingData(int start, int lenght, string searchdata);
        public Task<int> AddFillingDataAsync(AddFillingDataDTO data);
        public DBFillingData GetById(int id);
        public List<string> GetFillingDataNames();
        public DBFillingData GetFillingDataByName(string name);
        public Task DeleteAsync(List<DBFillingData> data);
    }
}