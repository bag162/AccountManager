using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace BASAccountManager
{
    public class HangfireAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            return httpContext.User.IsInRole("admin");
        }
    }
}