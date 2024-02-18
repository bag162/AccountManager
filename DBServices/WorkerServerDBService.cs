using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.Controllers.WorkerServer.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BASAccountManager.DBServices
{
    public class WorkerServerDBService : IWorkerServerDBService
    {
        private readonly IMapper mapper;
        public AMContext dbcontext { get; set; }

        public WorkerServerDBService(AMContext dbcontext, IMapper mapper)
        {
            this.dbcontext = dbcontext;
            this.mapper = mapper;

        }

        public async Task AddServerAsync(DBWorkerServer server)
        {
            this.dbcontext.WorkerServer.Add(server);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteServerAsync(int id)
        {
            var deletedServer = await this.dbcontext.WorkerServer.FindAsync(id);
            this.dbcontext.WorkerServer.Remove(deletedServer);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateServerAsync(DBWorkerServer updatedServer)
        {
            this.dbcontext.WorkerServer.Update(updatedServer);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public bool CheckForAccess(string apikey)
        {
            return this.dbcontext.WorkerServer
                .AsQueryable()
                .Where(x => x.APIKey == apikey)
                .Where(x => x.WorkerServerStatus == WorkerServerStatus.Active)
                .Count() != 0;
        }

        public List<DBWorkerServer> GetAllWorkerServers()
        {
            return this.dbcontext.WorkerServer.ToList();
        }

        public async Task DeleteServerAsync(int[] iDs)
        {
            var servers = this.dbcontext.WorkerServer.Where(x => iDs.Contains(x.Id)).ToList();
            this.dbcontext.WorkerServer.RemoveRange(servers);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<GetWorkerServerDTO> GetWorkerServer(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<GetWorkerServerDTO>();
            data.recordsTotal = this.dbcontext.WorkerServer.Count();
            List<DBWorkerServer> filteredData = new List<DBWorkerServer>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                filteredData = this.dbcontext.WorkerServer.AsQueryable().Where(m =>
                                                                    m.Name.Equals(searchdata)
                                                                    || m.APIKey.Equals(searchdata)
                                                                    || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToList();

                if (lenght != -1)
                {
                    filteredData = filteredData.Take(lenght).ToList();
                }

                data.data = mapper.Map<List<GetWorkerServerDTO>>(filteredData);
                data.recordsFiltered = filteredData.Count();
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                var listData = this.dbcontext.WorkerServer
                    .AsQueryable()
                    .ToList();

                if (lenght == -1)

                {
                    listData = listData.Skip(start).Take(data.recordsTotal).ToList();
                }
                else
                {
                    listData = listData.Skip(start).Take(lenght).ToList();
                }
                data.data = mapper.Map<List<GetWorkerServerDTO>>(listData.ToArray());
                data.recordsFiltered = listData.Count();
            }
            return data;
        }

        public async Task ChangeServerStatusAsync(int[] Ids)
        {
            var servers = this.dbcontext.WorkerServer.Where(x => Ids.Contains(x.Id)).ToList();
            foreach (var server in servers)
            {
                if (server.WorkerServerStatus == WorkerServerStatus.Active)
                {
                    server.WorkerServerStatus = WorkerServerStatus.Inactive;
                }
                else
                {
                    server.WorkerServerStatus = WorkerServerStatus.Active;
                }
            }
            
            this.dbcontext.WorkerServer.UpdateRange(servers);
            await this.dbcontext.SaveChangesAsync();
        }
    }
}