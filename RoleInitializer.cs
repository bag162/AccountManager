using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BASAccountManager
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("/proxy") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/proxy"));
            }
            if (await roleManager.FindByNameAsync("/Instaccount") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/Instaccount"));
            }
            if (await roleManager.FindByNameAsync("/smsservice") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/smsservice"));
            }
            if (await roleManager.FindByNameAsync("/email") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/email"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/taskdata") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/taskdata"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/registration") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/registration"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/authorization") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/authorization"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/posting") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/posting"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/commenting") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/commenting"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/liking") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/liking"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/folowing") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/folowing"));
            }
            if (await roleManager.FindByNameAsync("/post/group") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/group"));
            }
            if (await roleManager.FindByNameAsync("/post/manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/manager"));
            }
            if (await roleManager.FindByNameAsync("/post/comment/group") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/comment/group"));
            }
            if (await roleManager.FindByNameAsync("/post/comment/manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/comment/manager"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/workertaskdata") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/workertaskdata"));
            }
            if (await roleManager.FindByNameAsync("/post/manager/add") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/manager/add"));
            }
            if (await roleManager.FindByNameAsync("/post/manager/update") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/manager/update"));
            }
            
        }
    }
}