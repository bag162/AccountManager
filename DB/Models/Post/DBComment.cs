using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASAccountManager.DB.Models.Post
{
    public class DBComment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Message { get; set; }

        [ForeignKey(nameof(DBPostCommentGroup))]
        public int PostCommentGroupId { get; set; }
        public DBPostCommentGroup PostCommentGroup { get; set; }
    }
}