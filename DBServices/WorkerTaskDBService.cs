using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices
{
    public class WorkerTaskDBService : IWorkerTaskDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public WorkerTaskDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddWorkerTaskAsync(List<DBWorkerTask> addedTask)
        {
            foreach (var workerTask in addedTask)
            {
                workerTask.CreatedDate = DateTime.Now;
            }
            await this.dbcontext.WorkerTask.AddRangeAsync(addedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task<DBWorkerTask> GetWorkerByIdAsync(int id)
        {
            return await this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.Account).Include(x => x.Proxy).AsQueryable().Where(x => x.Id == id).FirstAsync();
        }

        public async Task<List<DBWorkerTask>> GetWorkerTaskByDBTaskIdAsync(int dbTaskId)
        {
            return await this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.Account).Include(x => x.Proxy).AsQueryable().Where(x => x.TaskId == dbTaskId).ToListAsync();
        }

        public JqueryDataTable<WorkerTaskDTO> GetWorkerTasks(int start, int lenght, string searchdata, int taskId)
        {
            var data = new JqueryDataTable<WorkerTaskDTO>();
            data.recordsTotal = this.dbcontext.WorkerTask.Count();
            DBWorkerTask[] filteredData = Array.Empty<DBWorkerTask>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    if (Enum.TryParse(searchdata, out DB.Models.TaskStatus result))
                    {
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.Account).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.TaskId == taskId).Where(m =>
                                                                        m.WorkerId.Equals(searchdata)
                                                                        || m.InstanceId.Contains(searchdata)
                                                                        || m.Account.Login.Contains(searchdata)
                                                                        || m.ProxyId.Equals(searchdata)
                                                                        || m.UsefulData.Contains(searchdata)
                                                                        || m.Status.Equals(result)
                                                                        || m.ErrorMessage.Contains(searchdata)
                                                                        || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.Account).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.TaskId == taskId).Where(m =>
                                                                        m.WorkerId.Equals(searchdata)
                                                                        || m.InstanceId.Contains(searchdata)
                                                                        || m.Account.Login.Contains(searchdata)
                                                                        || m.ProxyId.Equals(searchdata)
                                                                        || m.UsefulData.Contains(searchdata)
                                                                        || m.ErrorMessage.Contains(searchdata)
                                                                        || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                }
                else
                {
                    if (Enum.TryParse(searchdata, out DB.Models.TaskStatus result))
                    {
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.Account).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.TaskId == taskId).Where(m =>
                                                m.WorkerId.Equals(searchdata)
                                                || m.InstanceId.Contains(searchdata)
                                                || m.Account.Login.Contains(searchdata)
                                                || m.ProxyId.Equals(searchdata)
                                                || m.Status.Equals(result)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ErrorMessage.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.Account).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.TaskId == taskId).Where(m =>
                                                m.WorkerId.Equals(searchdata)
                                                || m.InstanceId.Contains(searchdata)
                                                || m.Account.Login.Contains(searchdata)
                                                || m.ProxyId.Equals(searchdata)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ErrorMessage.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                }


                data.recordsFiltered = filteredData.Count();
                var listData = filteredData.ToList();
                listData.Reverse();
                filteredData = listData.ToArray();
                data.data = mapper.Map<List<WorkerTaskDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    var listData = this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.Account).Include(x => x.Proxy).AsQueryable().Where(x => x.TaskId == taskId).Skip(start).Take(data.recordsTotal).ToList();
                    listData.Reverse();
                    data.data = mapper.Map<List<WorkerTaskDTO>>(listData.ToArray());
                }
                else
                {
                    var listData = this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.Account).Include(x => x.Proxy).AsQueryable().Where(x => x.TaskId == taskId).Skip(start).Take(lenght).ToList();
                    listData.Reverse();
                    data.data = mapper.Map<List<WorkerTaskDTO>>(listData.ToArray());
                }
            }
            return data;
        }

        public async Task<List<DBWorkerTask>> GetWorkerTasksAsync()
        {
            return await this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.Account).Include(x => x.Proxy).ToListAsync();
        }

        public async Task RemoveWorkerTaskAsync(List<DBWorkerTask> deletedTask)
        {
            var taskToDel = new List<DBWorkerTask>();
            foreach (var item in deletedTask)
            {
                if (item.Status != DB.Models.TaskStatus.AtWork)
                {
                    taskToDel.Add(item);
                }
            }
            this.dbcontext.WorkerTask.RemoveRange(deletedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateWorkerTaskAsync(DBWorkerTask updatedTask)
        {
            this.dbcontext.Update(updatedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
