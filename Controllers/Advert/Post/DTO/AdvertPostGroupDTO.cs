namespace BASAccountManager.Controllers.Advert.Post.DTO
{
    public class AdvertPostGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountPinnedPosts { get; set; }
    }

    public class AddAdvertPostGroupDTO
    {
        public string Name { get; set; }
    }

    public class DeleteAdvertPostGroupDTO
    {
        public int Id { get; set; }
    }
}
