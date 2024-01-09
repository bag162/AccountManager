namespace BASAccountManager.Controllers.Post.Comment.DTO
{
    public class CommentGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string CountComments { get; set; }
        public string CountPinnedPosts { get; set; }
    }

    public class CRUDCommentGroupDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
