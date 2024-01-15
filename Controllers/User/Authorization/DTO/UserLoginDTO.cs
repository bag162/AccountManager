using System.Diagnostics.Contracts;

namespace BASAccountManager.Controllers.User.Authorization.DTO
{
    public class UserLoginDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserDataDTO
    {
        public string Login { get; set; }
        public string[] Roles { get; set; }
        public string ErrorMessage { get; set; }
        public bool Error { get; set; }
    }

    public class CheckLoginDTO
    {
        public string Login { get; set; }
    }

}