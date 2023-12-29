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
        private ILogger<WorkerTaskDBService> logger;
        private IMapper mapper;

        public WorkerTaskDBService(AMContext amcontext, ILogger<WorkerTaskDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddWorkerTaskAsync(List<DBWorkerTask> addedTask)
        {
            await this.dbcontext.WorkerTask.AddRangeAsync(addedTask);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task<DBWorkerTask> GetWorkerByIdAsync(int id)
        {
            return await this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.InstAccount).Include(x => x.Proxy).AsQueryable().Where(x => x.Id == id).FirstAsync();
        }

        public async Task<List<DBWorkerTask>> GetWorkerTaskByDBTaskIdAsync(int dbTaskId)
        {
            return await this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.InstAccount).Include(x => x.Proxy).AsQueryable().Where(x => x.DBTaskId == dbTaskId).ToListAsync();
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
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.InstAccount).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.DBTaskId == taskId).Where(m =>
                                                                        m.WorkerId.Equals(searchdata)
                                                                        || m.InstanceId.Contains(searchdata)
                                                                        || m.InstAccount.Login.Contains(searchdata)
                                                                        || m.ProxyId.Equals(searchdata)
                                                                        || m.UsefulData.Contains(searchdata)
                                                                        || m.Status.Equals(result)
                                                                        || m.ErrorMessage.Contains(searchdata)
                                                                        || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.InstAccount).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.DBTaskId == taskId).Where(m =>
                                                                        m.WorkerId.Equals(searchdata)
                                                                        || m.InstanceId.Contains(searchdata)
                                                                        || m.InstAccount.Login.Contains(searchdata)
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
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.InstAccount).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.DBTaskId == taskId).Where(m =>
                                                m.WorkerId.Equals(searchdata)
                                                || m.InstanceId.Contains(searchdata)
                                                || m.InstAccount.Login.Contains(searchdata)
                                                || m.ProxyId.Equals(searchdata)
                                                || m.Status.Equals(result)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ErrorMessage.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.WorkerTask.Include(x => x.InstAccount).Include(x => x.Proxy).Include(x => x.Task).AsQueryable().Where(x => x.DBTaskId == taskId).Where(m =>
                                                m.WorkerId.Equals(searchdata)
                                                || m.InstanceId.Contains(searchdata)
                                                || m.InstAccount.Login.Contains(searchdata)
                                                || m.ProxyId.Equals(searchdata)
                                                || m.UsefulData.Contains(searchdata)
                                                || m.ErrorMessage.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<WorkerTaskDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<WorkerTaskDTO>>(this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.InstAccount).Include(x => x.Proxy).AsQueryable().Where(x => x.DBTaskId == taskId).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<WorkerTaskDTO>>(this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.InstAccount).Include(x => x.Proxy).AsQueryable().Where(x => x.DBTaskId == taskId).Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public async Task<List<DBWorkerTask>> GetWorkerTasksAsync()
        {
            return await this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.InstAccount).Include(x => x.Proxy).ToListAsync();
        }

        public async Task RemoveWorkerTaskAsync(List<DBWorkerTask> deletedTask)
        {
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
