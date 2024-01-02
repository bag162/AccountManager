using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBBASExeption
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime ExeptionTime { get; set; }
        public string ExeptionMessage { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int? AccountId { get; set; }
        public DBInstagramAccount? Account { get; set; }

        [ForeignKey(nameof(DBProxy))]
        public int? ProxyId { get; set; }
        public DBProxy? Proxy { get; set; }

        [ForeignKey(nameof(DBTask))]
        public int? TaskId { get; set; }
        public DBTask? Task { get; set; }
    }
}