using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            if (await roleManager.FindByNameAsync("/taskmanager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager"));
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
            if (await roleManager.FindByNameAsync("/taskmanager/profilefilling") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/profilefilling"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/advertliking") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/advertliking"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/advertfollowing") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/advertfollowing"));
            }
            if (await roleManager.FindByNameAsync("/taskmanager/advertcommenting") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskmanager/advertcommenting"));
            }
            if (await roleManager.FindByNameAsync("/profilefilling") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/profilefilling"));
            }
            if (await roleManager.FindByNameAsync("/profilefilling/add") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/profilefilling/add"));
            }
            if (await roleManager.FindByNameAsync("/profilefilling/view") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/profilefilling/view"));
            }
            if (await roleManager.FindByNameAsync("/advertresourses") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/advertresourses"));
            }
            if (await roleManager.FindByNameAsync("/advertresourses/account/group") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/advertresourses/account/group"));
            }
            if (await roleManager.FindByNameAsync("/advertresourses/account/manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/advertresourses/account/manager"));
            }
            if (await roleManager.FindByNameAsync("/advertresourses/post/group") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/advertresourses/post/group"));
            }
            if (await roleManager.FindByNameAsync("/advertresourses/post/manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/advertresourses/post/manager"));
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
            if (await roleManager.FindByNameAsync("/post") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post"));
            }
            if (await roleManager.FindByNameAsync("/post/manager/add") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/manager/add"));
            }
            if (await roleManager.FindByNameAsync("/post/manager/update") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/post/manager/update"));
            }
            if (await roleManager.FindByNameAsync("/InstGroup") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/InstGroup"));
            }
            if (await roleManager.FindByNameAsync("/proxygroup") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/proxygroup"));
            }
            if (await roleManager.FindByNameAsync("/dashboard") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/dashboard"));
            }
            if (await roleManager.FindByNameAsync("/taskscheduler") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskscheduler"));
            }
            if (await roleManager.FindByNameAsync("/taskscheduler/add") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskscheduler/add"));
            }
            if (await roleManager.FindByNameAsync("/taskscheduler/view") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("/taskscheduler/view"));
            }
        }
    }
}