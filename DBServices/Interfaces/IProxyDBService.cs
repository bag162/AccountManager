using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IProxyDBService
    {
        public List<DBProxy> GetProxy();
        public JqueryDataTable<ProxyDTO> GetProxy(int start, int lenght, string searchdata);
        public Task AddProxyAsync(List<DBProxy> newProxy);
        public Task RemoveProxyAsync(List<DBProxy> removedProxy);
        public Task UpdateProxyAsync(List<DBProxy> updatedProxy);
        public List<string> GetAllGroups();
        public Task AddGroupAsync(DBProxyGroup addedGroup);
    }
}