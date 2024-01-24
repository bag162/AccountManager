using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.AdvertResourses
{
    [Index(nameof(Name), IsUnique = true)]
    public class DBAdvertAccountGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DBAdvertAccount> ListAdvertAccount { get; set; }
    }
}
