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
        public Task RemoveTaskAsync(List<DBTask> removedTask);
        public Task RemoveTaskAsync(DBTask removedTask);
        public Task UpdateTaskAsync(List<DBTask> updatedTask);
        public Task UpdateTaskAsync(DBTask updatedTask);
    }
}