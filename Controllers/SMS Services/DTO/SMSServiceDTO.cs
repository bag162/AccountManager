using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.SMS_Services.DTO
{
    public class SMSServiceDTO
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string Country { get; set; }
        public string APIKey { get; set; }
    }
}