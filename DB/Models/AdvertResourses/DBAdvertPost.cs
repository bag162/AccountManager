using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.AdvertResourses
{
    public class DBAdvertPost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? LikingErrorMessage { get; set; }
        public string? CommentingErrorMessage { get; set; }

        public string PostURL { get; set; }
        public AdvertPostActionStatus AdvertPostLikeStatus { get; set; }
        public AdvertPostActionStatus AdvertPostCommentStatus { get; set; }

        [ForeignKey(nameof(DBAdvertPostGroup))]
        public int AdvertPostGroupId { get; set; }
        public DBAdvertPostGroup AdvertPostGroup { get; set; }
    }

    public enum AdvertPostActionStatus
    {
        NotProcessed,
        Processed,
        ProcessTreatment,
        Error
    }
}