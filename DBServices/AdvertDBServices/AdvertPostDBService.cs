using AutoMapper;
using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.Advert.Post.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.AdvertResourses;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.AdvertDBServices
{
    public class AdvertPostDBService : IAdvertPostDBService
    {
        private IMapper mapper;
        private AMContext dbcontext;

        public AdvertPostDBService(AMContext dbcontext, IMapper mapper)
        {
            this.dbcontext = dbcontext;
            this.mapper = mapper;
        }

        public async Task AddAdvertPostAsync(List<AddAdvertPostDTO> posts)
        {
            var groups = this.dbcontext.AdvertPostGroup.ToList();
            var postsToAdd = new List<DBAdvertPost>();
            foreach (var post in posts)
            {
                var accountToAdd = new DBAdvertPost()
                {
                    PostURL = post.PostURL,
                    AdvertPostGroupId = groups.Where(x => x.Name == post.AdvertPostGroupName).First().Id,
                    AdvertPostLikeStatus = AdvertPostActionStatus.NotProcessed,
                    AdvertPostCommentStatus = AdvertPostActionStatus.NotProcessed
                };
                postsToAdd.Add(accountToAdd);
            }
            await this.dbcontext.AdvertPost.AddRangeAsync(postsToAdd);
            await this.dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAdvertPostAsync(List<int> ids)
        {
            var deletedPosts = new List<DBAdvertPost>();
            foreach (var id in ids)
            {
                deletedPosts.Add(await this.dbcontext.AdvertPost.FindAsync(id));
            }

            this.dbcontext.AdvertPost.RemoveRange(deletedPosts);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<AdvertPostDTO> GetAdvertPost(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<AdvertPostDTO>();
            data.recordsTotal = this.dbcontext.AdvertPost.Count();
            DBAdvertPost[] filteredData = Array.Empty<DBAdvertPost>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Where(m =>
                                                m.PostURL.Contains(searchdata)
                                                || m.AdvertPostGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Where(m =>
                                                 m.PostURL.Contains(searchdata)
                                                 || m.AdvertPostGroup.Name.Contains(searchdata)
                                                 || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }

                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<AdvertPostDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<AdvertPostDTO>>(this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<AdvertPostDTO>>(this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public JqueryDataTable<AdvertPostDTO> GetAdvertPostByGroup(int start, int lenght, string searchdata, int groupId)
        {
            var data = new JqueryDataTable<AdvertPostDTO>();
            data.recordsTotal = this.dbcontext.AdvertPost.Count();
            DBAdvertPost[] filteredData = Array.Empty<DBAdvertPost>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Where(x => x.AdvertPostGroupId == groupId).Where(m =>
                                                m.PostURL.Contains(searchdata)
                                                || m.AdvertPostGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Where(x => x.AdvertPostGroupId == groupId).Where(m =>
                                                 m.PostURL.Contains(searchdata)
                                                 || m.AdvertPostGroup.Name.Contains(searchdata)
                                                 || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }

                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<AdvertPostDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<AdvertPostDTO>>(this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Where(x => x.AdvertPostGroupId == groupId).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<AdvertPostDTO>>(this.dbcontext.AdvertPost.Include(x => x.AdvertPostGroup).AsQueryable().Where(x => x.AdvertPostGroupId == groupId).Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public async Task<List<DBAdvertPost>> GetAdvertPostByGroupAsync(string groupName)
        {
            var groupId = await this.dbcontext.AdvertPostGroup.Where(x => x.Name == groupName).Select(x => x.Id).FirstAsync();
            return await this.dbcontext.AdvertPost.Where(x => x.AdvertPostGroupId == groupId).ToListAsync();
        }

        public DBAdvertPost GetAdvertPostById(int id)
        {
            return this.dbcontext.AdvertPost.Find(id);
        }

        public List<DBAdvertPost> GetAdvertPosts()
        {
            return this.dbcontext.AdvertPost.ToList();
        }

        public async Task UpdateAdvertPostAsync(List<DBAdvertPost> posts)
        {
            this.dbcontext.AdvertPost.UpdateRange(posts);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}