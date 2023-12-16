using BASAccountManager.Abstraction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBFacebookAccount : Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Group { get; set; }
        [ForeignKey(nameof(DBProxy))]
        public int? ProxyId { get; set; }

        public DBProxy? Proxy { get; set; }
    }
}