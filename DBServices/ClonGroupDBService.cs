using AutoMapper;
using BASAccountManager.Controllers.Clon.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BASAccountManager.DBServices
{
    public class ClonGroupDBService : IClonGroupDBService
    {
        private AMContext dbcontext { get; set; }
        private IMapper mapper { get; set; }

        public ClonGroupDBService(AMContext dbcontext, IMapper mapper)
        {
            this.dbcontext = dbcontext;
            this.mapper = mapper;
        }

        public JqueryDataTable<GetClonGroupDTO> GetGroups(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<GetClonGroupDTO>();
            data.recordsTotal = this.dbcontext.ClonGroup.Count();
            List<DBClonGroup> filteredData = new List<DBClonGroup>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                filteredData = this.dbcontext.ClonGroup.Include(x => x.ListClon).AsQueryable().Where(m => m.ListClon.Count().Equals(searchdata)
                                                                    || m.Name.Contains(searchdata)
                                                                    || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToList();

                if (lenght != -1)
                {
                    filteredData = filteredData.Take(lenght).ToList();
                }

                data.data = mapper.Map<List<GetClonGroupDTO>>(filteredData);
                data.recordsFiltered = filteredData.Count();
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                var listData = this.dbcontext.ClonGroup.Include(x => x.ListClon).AsQueryable().ToList();

                if (lenght == -1)

                {
                    listData = listData.Skip(start).Take(data.recordsTotal).ToList();
                }
                else
                {
                    listData = listData.Skip(start).Take(lenght).ToList();
                }
                data.data = mapper.Map<List<GetClonGroupDTO>>(listData.ToArray());
                data.recordsFiltered = listData.Count();
            }
            return data;
        }

        public async Task<List<DBClonGroup>> GetGroups()
        {
            return await this.dbcontext.ClonGroup
                .Include(x => x.ListClon).ThenInclude(x => x.PostGroup).ThenInclude(x => x.ListPost)
                .Include(x => x.ListClon).ThenInclude(x => x.ListInstAccount).ThenInclude(x => x.ListPost)
                .ToListAsync();
        }

        public async Task<int> AddClonGroupAsync(AddClonGroupDTO data)
        {
            var newGroup = new DBClonGroup()
            {
                Name = data.Name
            };
            await this.dbcontext.ClonGroup.AddAsync(newGroup);
            await this.dbcontext.SaveChangesAsync();
            return this.dbcontext.ClonGroup.Where(x => x.Name == newGroup.Name).First().Id;
        }

        public List<string> GetGroupNames()
        {
            return this.dbcontext.ClonGroup.Select(x => x.Name).ToList();
        }

        public DBClonGroup GetGroupByName(string name)
        {
            return this.dbcontext.ClonGroup
                .Include(x => x.ListClon).ThenInclude(x => x.ListInstAccount).ThenInclude(x => x.ListPost).ThenInclude(x => x.ListLikes)
                .Include(x => x.ListClon).ThenInclude(x => x.ListInstAccount).ThenInclude(x => x.ListPost).ThenInclude(x => x.ListComment).ThenInclude(x => x.Comment)
                .Include(x => x.ListClon).ThenInclude(x => x.FillingData)
                .Where(x => x.Name == name).First();
        }
    }
}
