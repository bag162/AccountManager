using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using BASAccountManager.Services;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class PostDBService : IPostDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public PostDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddPostAsync(CRUDPostDTO post)
        {
            var addedPost = this.mapper.Map<DBPost>(post);
            var postGroupId = this.dbcontext.PostGroup.Where(x => x.Name == post.PostGroupName).Select(x => x.Id).First();
            var commentGroupId = this.dbcontext.CommentGroup.Where(x => x.Name == post.CommentGroupName).Select(x => x.Id).First();
            addedPost.PostCommentGroupId = commentGroupId;
            addedPost.GroupId = postGroupId;
            addedPost.CreatedDate = DateTime.Now;
            var path = "wwwroot/images/posts/" + post.PostGroupName;

            string filePath = await ImageService.AddBase64ImageAsync(post.ImageFormat, post.ImageBase64, path, post.Name);
            addedPost.ImagePath = filePath.Remove(0, 8);

            await this.dbcontext.Post.AddAsync(addedPost);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeletePostAsync(List<DBPost> posts)
        {
            this.dbcontext.RemoveRange(posts);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<PostListDTO> GetPost(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<PostListDTO>();
            data.recordsTotal = this.dbcontext.Post.Count();
            DBPost[] filteredData = Array.Empty<DBPost>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Where(m => m.Group.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Where(m => m.Group.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<PostListDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<PostListDTO>>(this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<PostListDTO>>(this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public CRUDPostDTO GetPostById(int postId)
        {
            var post = this.dbcontext.Post.Include(x => x.Group).Include(x => x.PostCommentGroup).Where(x => x.Id == postId).First();
            return mapper.Map<CRUDPostDTO>(post);
        }

        public JqueryDataTable<PostListDTO> GetPostByPostGroup(int start, int lenght, string searchdata, int groupId)
        {
            var data = new JqueryDataTable<PostListDTO>();
            data.recordsTotal = this.dbcontext.Post.Where(x => x.GroupId == groupId).Count();
            DBPost[] filteredData = Array.Empty<DBPost>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Where(m => m.Group.Id.Equals(groupId)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Where(m => m.Group.Id.Equals(groupId)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<PostListDTO>>(filteredData);
            }
            else
            {
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<PostListDTO>>(this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Where(x => x.Group.Id.Equals(groupId)).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<PostListDTO>>(this.dbcontext.Post.Include(x => x.Group).Include(x => x.Group.AccountGroup).AsQueryable().Where(x => x.Group.Id.Equals(groupId)).Skip(start).Take(lenght).ToArray());
                }
                data.recordsFiltered = data.data.Count();
            }
            return data;
        }

        public async Task UpdatePostAsync(UpdatePostDTO post)
        {
            var updatedPost = this.dbcontext.Post.Find(post.Id);

            updatedPost.RequiredCountComments = post.RequiredCountComments;
            updatedPost.RequiredCountLikes = post.RequiredCountLikes;
            updatedPost.PostStatus = (PostStatus)Enum.Parse(typeof(PostStatus), post.PostStatus);

            this.dbcontext.Post.Update(updatedPost);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
