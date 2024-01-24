using AutoMapper;
using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.AdvertResourses;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.AdvertDBServices
{
    public class AdvertAccountGroupDBService : IAdvertAccountGroupDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;
        public AdvertAccountGroupDBService(AMContext dbcontext, IMapper mapper)
        {
            this.dbcontext = dbcontext;
            this.mapper = mapper;
        }
        
        public async Task AddAdvertAccountGroupAsync(AddAdvertAccountGroupDTO group)
        {
            var newGroup = new DBAdvertAccountGroup()
            {
                Name = group.Name
            };
            await this.dbcontext.AdvertAccountGroup.AddAsync(newGroup);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteAdvertAccountGroupAsync(List<DeleteAdvertAccountGroupDTO> groups)
        {
            var groupsToRemove = new List<DBAdvertAccountGroup>();
            foreach (var item in groups)
            {
                groupsToRemove.Add(await this.dbcontext.AdvertAccountGroup.FindAsync(item.Id));
            }
            this.dbcontext.AdvertAccountGroup.RemoveRange(groupsToRemove);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task<string[]> GetAdvertAccountGroupNamesAsync()
        {
            return await this.dbcontext.AdvertAccountGroup.Select(x => x.Name).ToArrayAsync();
        }

        public JqueryDataTable<AdvertAccountGroupDTO> GetAdvertAccountGroups(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<AdvertAccountGroupDTO>();
            data.recordsTotal = this.dbcontext.AdvertAccountGroup.Count();
            DBAdvertAccountGroup[] filteredData = Array.Empty<DBAdvertAccountGroup>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.AdvertAccountGroup.Include(x => x.ListAdvertAccount).AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.AdvertAccountGroup.Include(x => x.ListAdvertAccount).AsQueryable().Where(m =>  m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<AdvertAccountGroupDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<AdvertAccountGroupDTO>>(this.dbcontext.AdvertAccountGroup.Include(x => x.ListAdvertAccount).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<AdvertAccountGroupDTO>>(this.dbcontext.AdvertAccountGroup.Include(x => x.ListAdvertAccount).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }
    }
}