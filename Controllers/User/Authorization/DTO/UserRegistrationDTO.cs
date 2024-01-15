namespace BASAccountManager.Controllers.User.Authorization.DTO
{
    public class UserRegistrationDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class RegistrationUserDataDTO
    {
        public string Login { get; set; }
        public string ErrorMessage { get; set; }
        public bool Error { get; set; }
    }
}
