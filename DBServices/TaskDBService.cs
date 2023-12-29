using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.DBServices
{
    public class TaskDBService : ITaskDBService
    {
        private AMContext dbcontext;
        private ILogger<TaskDBService> logger;
        private IMapper mapper;

        public TaskDBService(AMContext amcontext, ILogger<TaskDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddTaskAsync(List<DBTask> newTask)
        {
            await this.dbcontext.Task.AddRangeAsync(newTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task AddTaskAsync(DBTask newTask)
        {
            await this.dbcontext.Task.AddAsync(newTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<DBTask> GetTask()
        {
            return this.dbcontext.Task.ToList();
        }

        public JqueryDataTable<TaskDTO> GetTask(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<TaskDTO>();
            data.recordsTotal = this.dbcontext.Task.Count();
            DBTask[] filteredData = Array.Empty<DBTask>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Task.AsQueryable().Where(m => m.ProxyGroup.Contains(searchdata)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ClientTaskName.Contains(searchdata)
                                                || m.AccountGroup.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Task.AsQueryable().Where(m => m.ProxyGroup.Contains(searchdata)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ClientTaskName.Contains(searchdata)
                                                || m.AccountGroup.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<TaskDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<TaskDTO>>(this.dbcontext.Task.AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<TaskDTO>>(this.dbcontext.Task.AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public DBTask GetTaskById(int id)
        {
            return this.dbcontext.Task.Find(id);
        }

        public async Task RemoveTaskAsync(List<DBTask> removedTask)
        {
            this.dbcontext.Task.RemoveRange(removedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task RemoveTaskAsync(DBTask removedTask)
        {
            this.dbcontext.Task.Remove(removedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateTaskAsync(List<DBTask> updatedTask)
        {
            this.dbcontext.Task.UpdateRange(updatedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateTaskAsync(DBTask updatedTask)
        {
            this.dbcontext.Task.Update(updatedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
