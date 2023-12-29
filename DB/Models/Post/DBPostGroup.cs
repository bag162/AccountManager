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

        [ForeignKey(nameof(DBInstagramAccount))]
        public int InstAccountId { get; set; }
        public DBInstagramAccount InstAccount { get; set; }
    }
}
