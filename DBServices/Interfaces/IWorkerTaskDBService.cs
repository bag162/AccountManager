using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IWorkerTaskDBService
    {
        public Task AddWorkerTaskAsync(List<DBWorkerTask> addedTask);
        public JqueryDataTable<WorkerTaskDTO> GetWorkerTasks(int start, int lenght, string searchdata, int taskId);
        public Task<List<DBWorkerTask>> GetWorkerTasksAsync();
        public Task<DBWorkerTask> GetWorkerByIdAsync(int id);
        public Task<List<DBWorkerTask>> GetWorkerTaskByDBTaskIdAsync(int dbTaskId);
        public Task RemoveWorkerTaskAsync(List<DBWorkerTask> deletedTask);
        public Task UpdateWorkerTaskAsync(DBWorkerTask updatedTask);
    }
}
