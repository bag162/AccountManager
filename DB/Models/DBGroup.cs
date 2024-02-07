using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class DBProxyGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DBProxy> Proxies { get; set; } = new List<DBProxy>();
    }

    [Index(nameof(Name), IsUnique = true)]
    public class DBInstAccountGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DBInstagramAccount> InstAccounts { get; set; } = new List<DBInstagramAccount>();
    }

    [Index(nameof(Name), IsUnique = true)]
    public class DBClonGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DBClon> ListClon { get; set; }
    }
}