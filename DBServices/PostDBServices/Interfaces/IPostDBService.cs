using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.PostDBServices.Interfaces
{
    public interface IPostDBService
    {
        public JqueryDataTable<PostListDTO> GetPost(int start, int lenght, string searchdata);
        public JqueryDataTable<PostListDTO> GetPostByPostGroup(int start, int lenght, string searchdata, int groupId);
        public JqueryDataTable<PostListDTO> GetPostByClonId(int start, int lenght, string searchdata, int clonId);
        public CRUDPostDTO GetPostById(int postId);
        public Task AddPostAsync(CRUDPostDTO posts);
        public Task DeletePostAsync(List<DBPost> posts);
        public Task UpdatePostAsync(UpdatePostDTO post);
    }
}