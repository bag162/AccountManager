using AutoMapper;
using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.Advert.Post.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.AdvertResourses;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.AdvertDBServices
{
    public class AdvertPostGroupDBService : IAdvertPostGroupDBService
    {
        private IMapper mapper;
        private AMContext dbcontext;

        public AdvertPostGroupDBService(AMContext dbcontext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbcontext = dbcontext;
        }

        public async Task AddAdvertPostGroupAsync(AddAdvertPostGroupDTO groups)
        {
            var newPost = new DBAdvertPostGroup()
            {
                Name = groups.Name
            };
            await this.dbcontext.AdvertPostGroup.AddAsync(newPost);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteAdvertPostGroupAsync(List<DeleteAdvertPostGroupDTO> groups)
        {
            var deletedGroups = new List<DBAdvertPostGroup>();
            foreach (var group in groups)
            {
                deletedGroups.Add(await this.dbcontext.AdvertPostGroup.FindAsync(group.Id));
            }
            this.dbcontext.AdvertPostGroup.RemoveRange(deletedGroups);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task<string[]> GetAdvertPostGroupNamesAsync()
        {
            return await this.dbcontext.AdvertPostGroup.Select(x => x.Name).ToArrayAsync();
        }

        public JqueryDataTable<AdvertPostGroupDTO> GetAdvertPostGroups(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<AdvertPostGroupDTO>();
            data.recordsTotal = this.dbcontext.AdvertPostGroup.Count();
            DBAdvertPostGroup[] filteredData = Array.Empty<DBAdvertPostGroup>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.AdvertPostGroup.Include(x => x.ListAdvertPost).AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.AdvertPostGroup.Include(x => x.ListAdvertPost).AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<AdvertPostGroupDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<AdvertPostGroupDTO>>(this.dbcontext.AdvertPostGroup.Include(x => x.ListAdvertPost).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<AdvertPostGroupDTO>>(this.dbcontext.AdvertPostGroup.Include(x => x.ListAdvertPost).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }
    }
}