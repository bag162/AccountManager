using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface ITaskDBService
    {
        public DBTask GetTaskById(int id);
        public List<DBTask> GetTask();
        public JqueryDataTable<TaskDTO> GetTask(int start, int lenght, string searchdata);
        public Task AddTaskAsync(List<DBTask> newTask);
        public Task AddTaskAsync(DBTask newTask);
        public Task<bool> RemoveTaskAsync(List<DBTask> removedTask);
        public Task<bool> RemoveTaskAsync(DBTask removedTask);
        public Task UpdateTaskAsync(List<DBTask> updatedTask);
        public Task UpdateTaskAsync(DBTask updatedTask);

        public Task<bool> StartTaskAsync(TaskDTO[] data);
        public Task<bool> StopTaskAsync(TaskDTO[] data);

        public Task<bool> StartTaskAsync(DBTask[] data);
        public Task<bool> StopTaskAsync(DBTask[] data);
    }
}