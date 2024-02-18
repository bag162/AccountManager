using AutoMapper;
using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.AdvertResourses;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.AdvertDBServices
{
    public class AdvertAccountDBService : IAdvertAccountDBService
    {
        private IMapper mapper;
        private AMContext dbcontext;

        public AdvertAccountDBService(AMContext dbcontext, IMapper mapper)
        {
            this.dbcontext = dbcontext;
            this.mapper = mapper;

        }

        public async Task AddAdvertAccountAsync(List<AddAdvertAccountDTO> accounts)
        {
            var groups = this.dbcontext.AdvertAccountGroup.ToList();
            var accsToAdd = new List<DBAdvertAccount>();
            foreach (var item in accounts)
            {
                var accountToAdd = new DBAdvertAccount()
                {
                    AccountURL = item.AccountURL,
                    AdvertAccountGroupId = groups.Where(x => x.Name == item.AdvertAccountGroupName).First().Id,
                    AdvertAccountStatus = AdvertAccountStatus.NotProcessed
                };
                accsToAdd.Add(accountToAdd);
            }
            await this.dbcontext.AdvertAccount.AddRangeAsync(accsToAdd);
            await this.dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAdvertAccountAsync(List<int> ids)
        {
            var accounts = new List<DBAdvertAccount>();
            foreach (var item in ids)
            {
                accounts.Add(await this.dbcontext.AdvertAccount.FindAsync(item));
            }
            this.dbcontext.AdvertAccount.RemoveRange(accounts);
            await this.dbcontext.SaveChangesAsync();
        }

        public JqueryDataTable<AdvertAccountDTO> GetAdvertAccount(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<AdvertAccountDTO>();
            data.recordsTotal = this.dbcontext.AdvertAccount.Count();
            DBAdvertAccount[] filteredData = Array.Empty<DBAdvertAccount>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    if (Enum.TryParse(searchdata, out AdvertAccountStatus result))
                    {
                        filteredData = this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Where(m =>
                                                m.AccountURL.Contains(searchdata)
                                                || m.AdvertAccountStatus.Equals(result)
                                                || m.AdvertAccountGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Where(m => 
                                                m.AccountURL.Contains(searchdata)
                                                || m.AdvertAccountGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                        
                }
                else
                {

                    filteredData = this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Where(m =>
                                                m.AccountURL.Contains(searchdata)
                                                || m.AdvertAccountGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<AdvertAccountDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<AdvertAccountDTO>>(this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<AdvertAccountDTO>>(this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public JqueryDataTable<AdvertAccountDTO> GetAdvertAccountByGroup(int start, int lenght, string searchdata, int groupId)
        {
            var data = new JqueryDataTable<AdvertAccountDTO>();
            data.recordsTotal = this.dbcontext.AdvertAccount.Count();
            DBAdvertAccount[] filteredData = Array.Empty<DBAdvertAccount>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    if (Enum.TryParse(searchdata, out AdvertAccountStatus result))
                    {
                        filteredData = this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Where(x => x.AdvertAccountGroupId == groupId).Where(m =>
                                                m.AccountURL.Contains(searchdata)
                                                || m.AdvertAccountStatus.Equals(result)
                                                || m.AdvertAccountGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Where(x => x.AdvertAccountGroupId == groupId).Where(m =>
                                                m.AccountURL.Contains(searchdata)
                                                || m.AdvertAccountGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }

                }
                else
                {

                    filteredData = this.dbcontext.AdvertAccount.Include(x => x.AdvertAccountGroup).AsQueryable().Where(x => x.AdvertAccountGroupId == groupId).Where(m =>
                                                m.AccountURL.Contains(searchdata)
                                                || m.AdvertAccountGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<AdvertAccountDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<AdvertAccountDTO>>(this.dbcontext.AdvertAccount
                        .Include(x => x.AdvertAccountGroup)
                        .Where(x => x.AdvertAccountGroupId == groupId)
                        .AsQueryable().Skip(start)
                        .Take(data.recordsTotal)
                        .ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<AdvertAccountDTO>>(this.dbcontext.AdvertAccount
                        .Include(x => x.AdvertAccountGroup)
                        .Where(x => x.AdvertAccountGroupId == groupId)
                        .AsQueryable()
                        .Skip(start)
                        .Take(lenght)
                        .ToArray());
                }
            }
            return data;
        }

        public DBAdvertAccount GetAdvertAccountById(int id)
        {
            return this.dbcontext.AdvertAccount.Find(id);
        }

        public DBAdvertAccount GetAdvertAccountByURI(string uri)
        {
            return this.dbcontext.AdvertAccount.Where(x => x.AccountURL == uri).First();
        }

        public List<DBAdvertAccount> GetAdvertAccounts()
        {
            return this.dbcontext.AdvertAccount.ToList();
        }

        public async Task<List<DBAdvertAccount>> GetAdvertAccountsByGroupAsync(string groupName)
        {
            var groupId = await this.dbcontext.AdvertAccountGroup.Where(x => x.Name == groupName).Select(x => x.Id).FirstAsync();

            return this.dbcontext.AdvertAccount.Where(x => x.AdvertAccountGroupId == groupId).ToList();
        }

        public async Task UpdateAvertAccountsAsync(List<DBAdvertAccount> accounts)
        {
            this.dbcontext.AdvertAccount.UpdateRange(accounts);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}