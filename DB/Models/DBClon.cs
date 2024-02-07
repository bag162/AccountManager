using BASAccountManager.DB.Models.Post;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BASAccountManager.DB.Models
{
    [Index(nameof(ClonURI), IsUnique = true)]
    public class DBClon
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClonURI { get; set; }
        public ClonStatus ClonStatus { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey(nameof(DBFillingData))]
        public int? FillingDataId { get; set; }
        public DBFillingData? FillingData { get; set; }

        [ForeignKey(nameof(DBPostGroup))]
        public int? PostGroupId { get; set; }
        public DBPostGroup? PostGroup { get; set; }

        [ForeignKey(nameof(DBClonGroup))]
        public int ClonGroupId { get; set; }
        public DBClonGroup ClonGroup { get; set; }

        public List<DBInstagramAccount> ListInstAccount { get; set; }
    }

    public enum ClonStatus
    {
        Processed,
        NotProcessed,
        NotAvailable
    }
}