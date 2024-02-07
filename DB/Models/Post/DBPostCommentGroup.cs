using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DB.Models.Post
{
    [Index(nameof(Name), IsUnique = true)]
    public class DBPostCommentGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DBComment> ListComment { get; set; }
        public List<DBPost> ListPost { get; set; }
    }
}