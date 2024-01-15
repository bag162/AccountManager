using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

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

        public async Task AddProxyAsync(List<DBProxy> newProxy, string group)
        {
            var groupId = this.dbcontext.ProxyGroup.Where(x => x.Name == group).Select(x => x.Id).First();
            foreach (var proxy in newProxy)
            {
                proxy.ProxyStatus = ProxyStatus.Free;
                proxy.ProxyGroupId = groupId;
                proxy.CreatedDate = DateTime.Now;
                if (proxy.ChangeIpURI != null)
                {
                    try
                    {
                        new Uri(proxy.ChangeIpURI);
                    }
                    catch (Exception)
                    {
                        throw new Exception("The URI is invalid. Enter an absolute URI\r\n​");
                    }
                    
                }
            }
            await this.dbcontext.Proxy.AddRangeAsync(newProxy.ToArray());
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<DBProxy> GetProxy()
        {
            return this.dbcontext.Proxy.Include(x => x.ProxyGroup).ToList();
        }

        public async Task RemoveProxyAsync(List<DBProxy> removedProxy)
        {
            var proxyToDelete = new List<DBProxy>();
            foreach (var proxy in removedProxy)
            {
                proxyToDelete.Add(this.dbcontext.Proxy.Include(x => x.BASExeptions).Where(x => x.Id == proxy.Id).First());
            }
            this.dbcontext.Proxy.RemoveRange(proxyToDelete);
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
                    if (Enum.TryParse(searchdata, out ProxyStatus result))
                    {
                        filteredData = this.dbcontext.Proxy.Include(x => x.ProxyGroup).AsQueryable().Where(m => m.Ip.Contains(searchdata)
                                                || m.Port.Contains(searchdata)
                                                || m.Login.Contains(searchdata)
                                                || m.ProxyGroup.Name.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.ProxyStatus.Equals(result)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.Proxy.Include(x => x.ProxyGroup).AsQueryable().Where(m => m.Ip.Contains(searchdata)
                                                || m.Port.Contains(searchdata)
                                                || m.Login.Contains(searchdata)
                                                || m.ProxyGroup.Name.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }

                }
                else
                {
                    if (Enum.TryParse(searchdata, out ProxyStatus result))
                    {
                        filteredData = this.dbcontext.Proxy.Include(x => x.ProxyGroup).AsQueryable().Where(m => m.Ip.Contains(searchdata)
                                            || m.Port.Contains(searchdata)
                                            || m.Login.Contains(searchdata)
                                            || m.ProxyGroup.Name.Contains(searchdata)
                                            || m.Password.Contains(searchdata)
                                            || m.ProxyStatus.Equals(result)
                                            || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.Proxy.Include(x => x.ProxyGroup).AsQueryable().Where(m => m.Ip.Contains(searchdata)
                                            || m.Port.Contains(searchdata)
                                            || m.Login.Contains(searchdata)
                                            || m.ProxyGroup.Name.Contains(searchdata)
                                            || m.Password.Contains(searchdata)
                                            || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<ProxyDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<ProxyDTO>>(this.dbcontext.Proxy.Include(x => x.ProxyGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<ProxyDTO>>(this.dbcontext.Proxy.Include(x => x.ProxyGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
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

        public List<DBProxy> GetProxyByGroup(string groupName)
        {
            var group = this.dbcontext.ProxyGroup.FirstOrDefault(x => x.Name == groupName);
            return this.dbcontext.Proxy.Include(x => x.ProxyGroup).Where(x => x.ProxyGroupId == group.Id).ToList();
        }

        public async Task UpdateProxyAsync(List<DBProxy> updatedProxy, string newGroup)
        {
            var groupId = this.dbcontext.ProxyGroup.Where(x => x.Name == newGroup).Select(x => x.Id).First();
            foreach (var proxy in updatedProxy)
            {
                proxy.ProxyStatus = ProxyStatus.Free;
                proxy.ProxyGroupId = groupId;
            }
            this.dbcontext.Proxy.UpdateRange(updatedProxy);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateProxyAsync(DBProxy updatedProxy)
        {
            this.dbcontext.Proxy.Update(updatedProxy);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task SetProxyFreeStatusAsync(int proxyId)
        {
            var proxy = this.dbcontext.Proxy.Find(proxyId);
            if (proxy.ProxyStatus == ProxyStatus.Free)
            {
                return;
            }
            if (proxy.ChangeIpURI != null)
            {
                HttpClient httpClient = new(){ BaseAddress = new Uri(proxy.ChangeIpURI) };
                await httpClient.GetAsync("");
            }

            proxy.ProxyStatus = ProxyStatus.Free;
            this.dbcontext.Proxy.Update(proxy);
            await this.dbcontext.SaveChangesAsync();
        }
    }
}