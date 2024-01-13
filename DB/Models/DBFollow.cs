using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASAccountManager.DB.Models
{
    public class DBFollow
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ErrorMessage { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int RecipientAccountId { get; set; }
        public DBInstagramAccount RecipientAccount { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int SenderAccountId { get; set; }
        public DBInstagramAccount SenderAccount { get; set; }

        public FollowStatus FollowStatus { get; set; }
    }

    public enum FollowStatus
    {
        Published,
        NotPublished,
        InProcessFollow,
        ErrorFollow
    }
}
