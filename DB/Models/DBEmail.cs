using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBEmail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? APIToken { get; set; }
        public string? MailDomain { get; set; }

        public EmailType EmailType { get; set; }
    }

    public enum EmailType
    {
        emailbase,
        kopeechkaStore
    }
}