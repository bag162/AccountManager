using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class RegistrationClientTaskWorkerUsefulDataSMSServiceDTO
    {
        public DBSMSActivation SMSServiceData { get; set; }
    }
    public class RegistrationClientTaskWorkerUsefulDataEmailServiceDTO
    {
        public DBEmail EmailServiceData { get; set; }
    }

    public class GetRegistrationTaskEmailServiceDTO
    {
        public int Id { get; set; }
        public DBProxy Proxy { get; set; }
        public RegistrationClientTaskWorkerUsefulDataEmailServiceDTO UsefulData { get; set; }
        public string TaskType { get; set; }
    }

    public class GetRegistrationTaskSMSServiceDTO
    {
        public int Id { get; set; }
        public DBProxy Proxy { get; set; }
        public RegistrationClientTaskWorkerUsefulDataSMSServiceDTO UsefulData { get; set; }
        public string TaskType { get; set; }
    }

    public class EndRegistrationTaskDTO
    {
        public int TaskWorkerId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ProfileLink { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}