using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Post
{
    public class DBPostComment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CommentTime { get; set; }

        [ForeignKey(nameof(DBInstPost))]
        public int PostId { get; set; }
        public DBInstPost Post { get; set; }

        [ForeignKey(nameof(DBComment))]
        public int CommentId { get; set; }
        public DBComment Comment { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int? AccountId { get; set; }
        public DBInstagramAccount? SenderAccount { get; set; }

        public CommentStatus CommentStatus { get; set; }
    }

    public enum CommentStatus
    {
        Published,
        NotPublished
    }
}