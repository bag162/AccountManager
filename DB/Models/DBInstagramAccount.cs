using BASAccountManager.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    [Index(nameof(Login), IsUnique = true)]
    public class DBInstagramAccount : Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? ProfileLink { get; set; } // TODO Add InstanceId Data
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [ForeignKey(nameof(DBInstAccountGroup))]
        public int GroupId { get; set; }
        public DBInstAccountGroup DBInstAccountGroup { get; set; }
    }
}