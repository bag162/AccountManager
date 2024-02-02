using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.SchedulerTask.DTO;
using BASAccountManager.DB.Models;
using Microsoft.Build.Framework;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface ISchedulerTaskDBService
    {
        public List<DBSchedulerTask> GetSchesulerTask();
        public Task<AddSchedulerTaskDTO> GetSchedulerTaskByIdAsync(int id);
        public JqueryDataTable<SchedulerTaskDTO> GetSchedulerTask(int start, int lenght, string searchdata);

        public Task<bool> AddSchedulerTaskAsync(AddSchedulerTaskDTO data);

        public Task DeleteSchedulerTaskAsync(int id);

        public Task StartSchedulerTasksAsync(int[] data);
        public Task StopSchedulerTasksAsync(int[] data);

        public Task UpdateAsync(UpdateSchedulerTaskDTO data);
        public Task UpdateAsync(DBSchedulerTask data);
    }
}