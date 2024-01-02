using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.Post.Post.DTO
{
    public class PostListDTO
    {
        public int Id { get; set; }
        public string? PostURI { get; set; }
        public string GroupName { get; set; }
        public string PostStatus { get; set; }
    }

    public class CRUDPostDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string GroupName { get; set; }
        public string PostStatus { get; set; }
        public int RequiredCountLikes { get; set; }
        public int RequiredCountComments { get; set; }
        public string? ImageBase64 { get; set; }
        public string? ImageFormat { get; set; }
        public string PostURI { get; set; }
        public string? ImagePath { get; set; }
    }

    public class UpdatePostDTO
    {
        public int Id { get; set; }
        public string PostStatus { get; set; }
        public int RequiredCountLikes { get; set; }
        public int RequiredCountComments { get; set; }
    }
}