using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Post
{
    public class DBPostLike
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(DBPost))]
        public int PostId { get; set; }
        public DBPost Post { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int AccountId { get; set; }
        public DBInstagramAccount Account { get; set; }

        public DateTime? LikeTime { get; set; }
        public LikeStatus LikeStatus { get; set; }
    }

    public enum LikeStatus
    {
        Published,
        NotPublished
    }
}