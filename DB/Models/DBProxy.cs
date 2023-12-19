using BASAccountManager.Abstraction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBProxy
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Ip { get; set; }
        public string Port { get; set; }

        public string? ChangeIpURI { get; set; }
        public int? StandartRotationSec { get; set; }

        public string? Login { get; set; }
        public string? Password { get; set; }

        [ForeignKey(nameof(DBProxyGroup))]
        public int GroupId { get; set; }
        public DBProxyGroup DBProxyGroup { get; set; }

        public ProxyStatus ProxyStatus { get; set; }
    }

    public enum ProxyStatus
    {
        Free,
        BookedForWork,
        InWork
    }
}