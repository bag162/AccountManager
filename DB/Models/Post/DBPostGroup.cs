using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.Post
{
    public class DBPostGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(DBInstAccountGroup))]
        public int AccountGroupId { get; set; }
        public DBInstAccountGroup AccountGroup { get; set; }

        public List<DBPost> Posts { get; set; } = new List<DBPost>();
    }
}