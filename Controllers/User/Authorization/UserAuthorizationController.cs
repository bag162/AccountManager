using AutoMapper;
using BASAccountManager.Controllers.InstTask;
using BASAccountManager.Controllers.User.Authorization.DTO;
using BASAccountManager.DB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.User.Login
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserAuthorizationController : ControllerBase
    {
        private IMapper mapper;
        private readonly ILogger<UserAuthorizationController> logger;
        private readonly UserManager<DBUser> userManager;
        private readonly SignInManager<DBUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserAuthorizationController(IMapper mapper, 
            ILogger<UserAuthorizationController> logger, 
            RoleManager<IdentityRole> roleManager,
            UserManager<DBUser> userManager,
            SignInManager<DBUser> signInManager)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<string> CheckFreeLogin([FromBody] CheckLoginDTO data)
        {
            if (userManager.Users.Where(x => x.UserName == data.Login).Count() == 0)
            {
                return JsonConvert.SerializeObject(true);
            }
            else
            {
                return JsonConvert.SerializeObject(false);
            }
        }

        [HttpPost]
        public async Task<string> Login([FromBody] UserLoginDTO user)
        {
            var result = await signInManager.PasswordSignInAsync(user.Login, user.Password, true, false);
            var returnedData = new LoginUserDataDTO();
            if (result.Succeeded)
            {
                returnedData.Login = user.Login;
                returnedData.Error = false;
                return JsonConvert.SerializeObject(returnedData);
            }
            else
            {
                returnedData.Error = true;
                returnedData.ErrorMessage = "Wrong login or password";
                return JsonConvert.SerializeObject(returnedData);
            }
        }

        /*[HttpGet]
        public async Task<string> GetRoles()
        {
            var currnedUser = await userManager.FindByLoginAsync(user.Login, user.Login);
            var roles = await userManager.GetRolesAsync(currnedUser);

            returnedData.Roles = roles.ToArray();
        }*/

        [HttpPost]
        public async Task<string> Registration([FromBody] UserRegistrationDTO user)
        {
            DBUser regUser = new() { UserName = user.Login, Email = user.Login };
            var result = await userManager.CreateAsync(regUser, user.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(regUser, true);

                var returnedData = new RegistrationUserDataDTO()
                {
                    Error = false,
                    Login = user.Login
                };
                return JsonConvert.SerializeObject(returnedData);
            }
            else
            {
                var returnedData = new RegistrationUserDataDTO()
                {
                    Error = true,
                    ErrorMessage = result.Errors.Select(x => x.Description).First().ToString()
                };
                return JsonConvert.SerializeObject(returnedData);
            }
        }
    }
}
