using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BASAccountManager.DB.Models.Post
{
    [Index(nameof(PostURI), IsUnique = true)]
    public class DBInstPost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? PostingErrorMessage { get; set; }
        public string? PostURI { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey(nameof(DBPost))]
        public int PostId { get; set; }
        public DBPost Post { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int AccountId { get; set; }
        public DBInstagramAccount Account { get; set; }

        public InstPostStatus InstPostStatus { get; set; }

        public List<DBPostComment> ListComment { get; set; }
        public List<DBPostLikes> ListLikes { get; set; }
    }

    public enum InstPostStatus
    {
        Published,
        NotPublished,
        PostingError,
        InProcessPublication
    }
}