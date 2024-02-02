using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Clon
{
    public class DBClon
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DBClonProfileData ClonProfileData { get; set; }
        public List<DBClonPost> ListClonPost { get; set; }
    }
}