using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Clon
{
    public class DBClonProfileData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(DBClon))]
        public int? ClonId { get; set; }
        public DBClon? Clon { get; set; }
    }
}