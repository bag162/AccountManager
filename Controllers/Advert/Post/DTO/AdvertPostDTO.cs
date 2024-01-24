namespace BASAccountManager.Controllers.Advert.Post.DTO
{
    public class AdvertPostDTO
    {
        public int Id { get; set; }
        public string PostURL { get; set; }
        public string AdvertPostLikeStatus { get; set; }
        public string AdvertPostCommentStatus { get; set; }
        public string AdvertPostGroupName { get; set; }
    }

    public class AddAdvertPostDTO
    {
        public string PostURL { get; set; }
        public string AdvertPostGroupName { get; set; }
    }

    public class DeleteAdvertPostDTO
    {
        public int Id { get; set; }
    }
}