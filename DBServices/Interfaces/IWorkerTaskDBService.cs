using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IWorkerTaskDBService
    {
        public Task AddWorkerTaskAsync(List<DBWorkerTask> addedTask);
        public Task<List<DBWorkerTask>> GetWorkerTaskByDBTaskIdAsync(int dbTaskId);
    }
}
