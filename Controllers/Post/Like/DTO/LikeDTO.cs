using BASAccountManager.DB.Models.Post;

namespace BASAccountManager.Controllers.Post.Like.DTO
{
    public class LikeDTO
    {
        public int Id { get; set; }
        public string AccountLogin { get; set; }
        public DateTime? LikeTime { get; set; }
        public string LikeStatus { get; set; }
    }
}