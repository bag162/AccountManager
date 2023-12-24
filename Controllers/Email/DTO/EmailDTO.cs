using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.Email.DTO
{
    public class EmailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailType { get; set; }
        public string? APIToken { get; set; }
        public string? MailDomain { get; set; }
    }
}