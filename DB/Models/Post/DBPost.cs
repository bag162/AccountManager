using BASAccountManager.DB.Models.Post;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBPost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public string? Description { get; set; }
        public string? PostURI { get; set; }
        public int RequiredCountLikes { get; set; }
        public int RequiredCountComments { get; set; }

        [ForeignKey(nameof(DBPostGroup))]
        public int GroupId { get; set; }
        public DBPostGroup Group { get; set; }

        [ForeignKey(nameof(DBPostCommentGroup))]
        public int PostCommentGroupId { get; set; }
        public DBPostCommentGroup PostCommentGroup { get; set; }

        public PostStatus PostStatus { get; set; }

        public List<DBInstPost> ListPost { get; set; } = new List<DBInstPost>();
    }

    public enum PostStatus
    {
        Active,
        Inactive
    }
}