using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.WorkerServer.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IWorkerServerDBService
    {
        public Task AddServerAsync(DBWorkerServer server);

        public Task DeleteServerAsync(int id);
        public Task DeleteServerAsync(int[] iDs);

        public Task UpdateServerAsync(DBWorkerServer updatedServer);
        public Task ChangeServerStatusAsync(int[] Ids);

        public bool CheckForAccess(string apikey);

        public List<DBWorkerServer> GetAllWorkerServers();
        public JqueryDataTable<GetWorkerServerDTO> GetWorkerServer(int start, int lenght, string searchdata);
    }
}