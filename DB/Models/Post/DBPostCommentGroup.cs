using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BASAccountManager.DB.Models.Post
{
    public class DBPostCommentGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DBComment> ListComment { get; set; }
        public List<DBPost> ListPost { get; set; }
    }
}