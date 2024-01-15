using BASAccountManager.Abstraction;
using BASAccountManager.DB.Models.Post;
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

        public string? InstanceId { get; set; }
        public string? ProfileLink { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [ForeignKey(nameof(DBInstAccountGroup))]
        public int InstGroupId { get; set; }
        public DBInstAccountGroup InstGroup { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public List<DBInstPost> ListPost { get; set; } = new List<DBInstPost>();
        public List<DBPostComment> ListComments { get; set; } = new List<DBPostComment>();
        public List<DBPostLikes> ListLikes { get; set; } = new List<DBPostLikes>();
    }

    public enum AccountStatus
    {
        Authorized,
        NotAuthorized,
        Banned,
        IncorrectCredentionalData
    }
}