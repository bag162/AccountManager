using Microsoft.AspNetCore.Identity;

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

            if (await roleManager.FindByNameAsync("email") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("email"));
            }

            if (await roleManager.FindByNameAsync("email") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("email"));
            }

            if (await roleManager.FindByNameAsync("instAccount") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("instAccount"));
            }

            if (await roleManager.FindByNameAsync("instGroup") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("instGroup"));
            }

            if (await roleManager.FindByNameAsync("post") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("post"));
            }

            if (await roleManager.FindByNameAsync("postComment") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("postComment"));
            }

            if (await roleManager.FindByNameAsync("postGroup") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("postGroup"));
            }

            if (await roleManager.FindByNameAsync("proxy") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("proxy"));
            }

            if (await roleManager.FindByNameAsync("proxyGroup") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("proxyGroup"));
            }

            if (await roleManager.FindByNameAsync("smsService") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("smsService"));
            }

            if (await roleManager.FindByNameAsync("taskManager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("taskManager"));
            }

            if (await roleManager.FindByNameAsync("task") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("task"));
            }
        }
    }
}