using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class DBFillingData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public FillingDataGender Gender { get; set; }
        public bool EnableRecomendations { get; set; }
        public bool ClosedAccount { get; set; }

        public string? AvatarPath { get; set; }
        public string? AboutMe { get; set; }
        public string? NameOrSurnameGenString { get; set; }
        public string? UsernameGenString { get; set; }

        public List<DBInstagramAccount> ListAccounts { get; set; }
    }

    public enum FillingDataGender
    {
        Male,
        Female
    }
}