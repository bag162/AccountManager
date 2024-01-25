using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models.AdvertResourses
{
    [Index(nameof(Name), IsUnique = true)]
    public class DBAdvertPostGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<DBAdvertPost> ListAdvertPost { get; set; }
    }
}