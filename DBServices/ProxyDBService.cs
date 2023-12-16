using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.DBServices
{
    public class ProxyDBService : IProxyDBService
    {
        private AMContext dbcontext;
        private ILogger<ProxyDBService> logger;
        private IMapper mapper;

        public ProxyDBService(AMContext amcontext, ILogger<ProxyDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddProxyAsync(List<DBProxy> newProxy)
        {
            await this.dbcontext.Proxy.AddRangeAsync(newProxy.ToArray());
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<DBProxy> GetProxy()
        {
            return this.dbcontext.Proxy.ToList();
        }


        public async Task RemoveProxyAsync(List<DBProxy> removedProxy)
        {
            this.dbcontext.Proxy.RemoveRange(removedProxy);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateProxyAsync(List<DBProxy> updatedProxy)
        {
            this.dbcontext.Proxy.UpdateRange(updatedProxy);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<ProxyDTO> GetProxy(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<ProxyDTO>();
            data.recordsTotal = this.dbcontext.Proxy.Count();
            DBProxy[] filteredData = Array.Empty<DBProxy>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Proxy.AsQueryable().Where(m => m.Ip.Contains(searchdata)
                                                || m.Port.Contains(searchdata)
                                                || m.Login.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.Id.Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Proxy.AsQueryable().Where(m => m.Ip.Contains(searchdata)
                                                || m.Port.Contains(searchdata)
                                                || m.Login.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.Id.Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }

                
                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<ProxyDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<ProxyDTO>>(this.dbcontext.Proxy.AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<ProxyDTO>>(this.dbcontext.Proxy.AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public List<string> GetAllGroups()
        {
            return this.dbcontext.ProxyGroup.Select(x => x.Name).ToList();
        }

        public async Task AddGroupAsync(DBProxyGroup addedGroup)
        {
            await this.dbcontext.ProxyGroup.AddAsync(addedGroup);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}