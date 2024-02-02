using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Clon
{
    public class DBClonPostLike
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(DBClonPost))]
        public int? ClonPostId { get; set; }
        public DBClonPost? ClonPost { get; set; }
    }
}
