using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Instagram.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices
{
    public class InstDBService : IInstDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public InstDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddInstAccountsAsync(List<DBInstagramAccount> newAccs, string group)
        {
            var Instaccgroup = this.dbcontext.InstAccountGroup.Where(x => x.Name == group).Select(x => x.Id).First();
            foreach (var item in newAccs)
            {
                item.ReceiptDate = DateTime.Now;
                item.InstGroupId = Instaccgroup;
            }

            await this.dbcontext.InstAccount.AddRangeAsync(newAccs.ToArray());
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task AddGroupAsync(DBInstAccountGroup addedGroup)
        {
            await this.dbcontext.InstAccountGroup.AddAsync(addedGroup);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<string> GetAllGroups()
        {
            return this.dbcontext.InstAccountGroup.Select(x => x.Name).ToList();
        }

        public List<DBInstagramAccount> GetInstAccounts()
        {
            return this.dbcontext.InstAccount.Include(x => x.InstGroup).ToList();
        }

        public JqueryDataTable<InstAccountDTO> GetInstAccounts(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<InstAccountDTO>();
            data.recordsTotal = this.dbcontext.InstAccount.Count();

            if (!string.IsNullOrEmpty(searchdata))
            {
                DBInstagramAccount[] filteredData = Array.Empty<DBInstagramAccount>();
                if (lenght == -1)
                {
                    if (Enum.TryParse(searchdata, out AccountStatus result))
                    {
                        filteredData = this.dbcontext.InstAccount.Include(x => x.InstGroup).AsQueryable().Where(m => m.Login.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.InstGroup.Name.Contains(searchdata)
                                                || m.AccountStatus.Equals(result)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.InstAccount.Include(x => x.InstGroup).AsQueryable().Where(m => m.Login.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.InstGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                    }
                }
                else
                {
                    if (Enum.TryParse(searchdata, out AccountStatus result))
                    {
                        filteredData = this.dbcontext.InstAccount.Include(x => x.InstGroup).AsQueryable().Where(m => m.Login.Contains(searchdata)
                                            || m.Password.Contains(searchdata)
                                            || m.InstGroup.Name.Contains(searchdata)
                                            || m.AccountStatus.Equals(result)
                                            || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                    else
                    {
                        filteredData = this.dbcontext.InstAccount.Include(x => x.InstGroup).AsQueryable().Where(m => m.Login.Contains(searchdata)
                                            || m.Password.Contains(searchdata)
                                            || m.InstGroup.Name.Contains(searchdata)
                                            || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                    }
                }

                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<InstAccountDTO>>(filteredData);
            }
            else
            {
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<InstAccountDTO>>(this.dbcontext.InstAccount.Include(x => x.InstGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<InstAccountDTO>>(this.dbcontext.InstAccount.Include(x => x.InstGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
                data.recordsFiltered = data.recordsTotal;
            }
            return data;
        }

        public async Task RemoveInstAccountsAsync(List<DBInstagramAccount> removedAccs)
        {
            var deleteAccounts = new List<DBInstagramAccount>();
            foreach (var removedAccount in removedAccs)
            {
                deleteAccounts.Add(this.dbcontext.InstAccount
                    .Include(x => x.ListComments)
                    .Include(x => x.ListPost)
                    .Include(x => x.ListLikes)
                    .Include(x => x.ListBASExeption)
                    .Where(x => x.Id == removedAccount.Id)
                    .First());
                var removedFollows = this.dbcontext.Follow.AsQueryable().Where(x => x.SenderAccountId == removedAccount.Id).ToList();
                this.dbcontext.Follow.RemoveRange(removedFollows);
            }
            this.dbcontext.InstAccount.RemoveRange(deleteAccounts);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateInstAccountsAsync(List<DBInstagramAccount> updatedAccs)
        {
            this.dbcontext.InstAccount.UpdateRange(updatedAccs);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateInstAccountsAsync(List<DBInstagramAccount> updatedProxy, string newGroup)
        {
            var Instaccgroup = this.dbcontext.InstAccountGroup.Where(x => x.Name == newGroup).Select(x => x.Id).First();
            foreach (var item in updatedProxy)
            {
                item.InstGroupId = Instaccgroup;
            }
            this.dbcontext.InstAccount.UpdateRange(updatedProxy);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task<int> AddInstAccountsAsync(DBInstagramAccount newAccount, string accGroup)
        {
            var Instaccgroup = this.dbcontext.InstAccountGroup.Where(x => x.Name == accGroup).Select(x => x.Id).First();
            newAccount.ReceiptDate = DateTime.Now;
            newAccount.InstGroupId = Instaccgroup;

            await this.dbcontext.InstAccount.AddAsync(newAccount);
            await this.dbcontext.SaveChangesAsync();
            return this.dbcontext.InstAccount.AsQueryable().Where(x => x.Login == newAccount.Login).Select(x => x.Id).First();
        }

        public async Task<List<DBInstagramAccount>> GetInstAccountsByGroupAsync(string group)
        {
            var groupId = this.dbcontext.InstAccountGroup.Where(x => x.Name == group).First().Id;
            return await this.dbcontext.InstAccount.Include(x => x.ListPost).Include(x => x.FillingData).AsQueryable().Where(x => x.InstGroupId == groupId).ToListAsync();
        }
    }
}