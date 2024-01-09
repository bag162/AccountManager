using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Post
{
    public class DBInstPost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LikesCount { get; set; }

        public string? PostingErrorMessage { get; set; }
        public string? PostURI { get; set; }

        [ForeignKey(nameof(DBPost))]
        public int PostId { get; set; }
        public DBPost Post { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int AccountId { get; set; }
        public DBInstagramAccount Account { get; set; }

        public InstPostStatus InstPostStatus { get; set; }

        public List<DBPostComment> ListComment { get; set; } = new List<DBPostComment>();
    }

    public enum InstPostStatus
    {
        Published,
        NotPublished,
        PostingError
    }
}