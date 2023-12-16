using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Only SMS Activate API
namespace BASAccountManager.DB.Models
{
    public class DBSMSActivation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ServiceName { get; set; }

        public string APIURI { get; set; }
        public string APIKey { get; set; }
    }
}