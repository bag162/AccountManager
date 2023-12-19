using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Facebook.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices
{
    public class FBDBService : IFBDBService
    {
        private AMContext dbcontext;
        private ILogger<FBDBService> logger;
        private IMapper mapper;

        public FBDBService(AMContext amcontext, ILogger<FBDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddFBAccountsAsync(List<DBFacebookAccount> newAccs, string group)
        {
            var fbaccgroup = this.dbcontext.FBAccountGroup.Where(x => x.Name == group).Select(x => x.Id).First();
            foreach (var item in newAccs)
            {
                item.ReceiptDate = DateTime.Now;
                item.GroupId = fbaccgroup;
            }

            await this.dbcontext.FBAccount.AddRangeAsync(newAccs.ToArray());
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task AddGroupAsync(DBFBAccountGroup addedGroup)
        {
            await this.dbcontext.FBAccountGroup.AddAsync(addedGroup);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<string> GetAllGroups()
        {
            return this.dbcontext.FBAccountGroup.Select(x => x.Name).ToList();
        }

        public List<DBFacebookAccount> GetFBAccounts()
        {
            return this.dbcontext.FBAccount.Include(x => x.DBFBAccountGroup).ToList();
        }

        public JqueryDataTable<FBAccountDTO> GetFBAccounts(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<FBAccountDTO>();
            data.recordsTotal = this.dbcontext.FBAccount.Count();

            if (!string.IsNullOrEmpty(searchdata))
            {
                DBFacebookAccount[] filteredData = Array.Empty<DBFacebookAccount>();
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.FBAccount.Include(x => x.DBFBAccountGroup).AsQueryable().Where(m => m.Login.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.Id.Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {
                    filteredData = this.dbcontext.FBAccount.Include(x => x.DBFBAccountGroup).AsQueryable().Where(m => m.Login.Contains(searchdata)
                                                || m.Password.Contains(searchdata)
                                                || m.Id.Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }

                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<FBAccountDTO>>(filteredData);
            }
            else
            {
                if(lenght == -1)
                {
                    data.data = mapper.Map<List<FBAccountDTO>>(this.dbcontext.FBAccount.Include(x => x.DBFBAccountGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<FBAccountDTO>>(this.dbcontext.FBAccount.Include(x => x.DBFBAccountGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
                data.recordsFiltered = data.recordsTotal;
            }
            return data;
        }

        public async Task RemoveFBAccountsAsync(List<DBFacebookAccount> removedAccs)
        {
            this.dbcontext.FBAccount.RemoveRange(removedAccs);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateFBAccountsAsync(List<DBFacebookAccount> updatedAccs)
        {
            this.dbcontext.FBAccount.UpdateRange(updatedAccs);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateFBAccountsAsync(List<DBFacebookAccount> updatedProxy, string newGroup)
        {
            var fbaccgroup = this.dbcontext.FBAccountGroup.Where(x => x.Name == newGroup).Select(x => x.Id).First();
            foreach (var item in updatedProxy)
            {
                item.GroupId = fbaccgroup;
            }
            this.dbcontext.FBAccount.UpdateRange(updatedProxy);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}