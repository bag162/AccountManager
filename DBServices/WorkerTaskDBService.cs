using AutoMapper;
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

        public async Task<List<DBWorkerTask>> GetWorkerTaskByDBTaskIdAsync(int dbTaskId)
        {
            return await this.dbcontext.WorkerTask.Include(x => x.Task).Include(x => x.AccountFacebook).Include(x => x.Proxy).Where( x => x.DBTaskId == dbTaskId).ToListAsync();
        }
    }
}
