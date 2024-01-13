using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Post
{
    public class DBPostLikes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ErrorMessage { get; set; }

        [ForeignKey(nameof(DBInstPost))]
        public int PostId { get; set; }
        public DBInstPost Post { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int? SenderAccountId { get; set; }
        public DBInstagramAccount? SenderAccount { get; set; }

        public LikeStatus LikeStatus { get; set; }
    }

    public enum LikeStatus
    {
        Published,
        NotPublished,
        InProcessPublication,
        ErrorPublication
    }
}
