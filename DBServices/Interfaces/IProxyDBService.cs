using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IProxyDBService
    {
        public List<DBProxy> GetProxy();
        public JqueryDataTable<ProxyDTO> GetProxy(int start, int lenght, string searchdata);
        public List<DBProxy> GetProxyByGroup(string groupName);
        public Task AddProxyAsync(List<DBProxy> newProxy, string groupName);
        public Task RemoveProxyAsync(List<DBProxy> removedProxy);
        public Task UpdateProxyAsync(DBProxy updatedProxy);
        public Task UpdateProxyAsync(List<DBProxy> updatedProxy);
        public Task UpdateProxyAsync(List<DBProxy> updatedProxy, string newGroup);
        public List<string> GetAllGroups();
        public Task AddGroupAsync(DBProxyGroup addedGroup);
        public Task SetProxyFreeStatusAsync(int proxyId);
    }
}