using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Post
{
    public class DBPostComment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Message { get; set; }

        [ForeignKey(nameof(DBPost))]
        public int PostId { get; set; }
        public DBPost Post { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int AccountId { get; set; }
        public DBInstagramAccount Account { get; set; }

        public DateTime? CommentTime { get; set; }
        public CommentStatus CommentStatus { get; set; }
    }

    public enum CommentStatus
    {
        Published,
        NotPublished
    }
}