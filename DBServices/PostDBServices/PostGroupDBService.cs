using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Group.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class PostGroupDBService : IPostGroupDBService
    {
        private AMContext dbcontext;
        private ILogger<PostGroupDBService> logger;
        private IMapper mapper;

        public PostGroupDBService(AMContext amcontext, ILogger<PostGroupDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddGroupsAsync(List<PostGroupDTO> groups)
        {
            var newGroups = new List<DBPostGroup>();

            foreach (var group in groups)
            {
                var newGroup = this.mapper.Map<DBPostGroup>(group);
                newGroup.AccountGroupId = this.dbcontext.InstAccountGroup.Where(x => x.Name == group.AccountGroupName).Select(x => x.Id).First();
            }
            await this.dbcontext.PostGroup.AddRangeAsync(newGroups);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteGroupsAsync(List<DBPostGroup> groups)
        {
            this.dbcontext.RemoveRange(groups);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<PostGroupDTO> GetGroups(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<PostGroupDTO>();
            data.recordsTotal = this.dbcontext.PostGroup.Count();
            DBPostGroup[] filteredData = Array.Empty<DBPostGroup>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.PostGroup.Include(x => x.AccountGroup).Include(x => x.ListPost).AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.PostGroup.Include(x => x.AccountGroup).Include(x => x.ListPost).AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<PostGroupDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<PostGroupDTO>>(this.dbcontext.PostGroup.Include(x => x.AccountGroup).Include(x => x.ListPost).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<PostGroupDTO>>(this.dbcontext.PostGroup.Include(x => x.AccountGroup).Include(x => x.ListPost).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public List<DBPostGroup> GetGroups()
        {
            return this.dbcontext.PostGroup.Include(x => x.AccountGroup).Include(x => x.ListPost).ToList();
        }

        public List<string> GetListGroupNames()
        {
            return this.dbcontext.PostGroup.AsQueryable().Select(x => x.Name).ToList();
        }
    }
}
