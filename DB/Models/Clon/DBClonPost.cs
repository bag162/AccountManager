using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Clon
{
    public class DBClonPost
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(DBClon))]
        public int? ClonId { get; set; }
        public DBClon? Clon { get; set; }

        public List<DBClonPostLike> ListClonLike { get; set; }
        public List<DBClonPostComment> ListClonComment { get; set; }
    }
}