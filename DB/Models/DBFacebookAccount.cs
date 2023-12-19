using BASAccountManager.Abstraction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBFacebookAccount : Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(DBFBAccountGroup))]
        public int GroupId { get; set; }
        public DBFBAccountGroup DBFBAccountGroup { get; set; }
    }
}