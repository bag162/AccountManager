using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.Middlewares
{
    public class BASAPIAccessManager
    {
        private readonly RequestDelegate next;

        public BASAPIAccessManager(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IWorkerServerDBService workerServerDBService)
        {
            if (context.Request.Path.HasValue == false)
            {
                await next.Invoke(context);
            }

            var pathRoutes = context.Request.Path.Value.Split('/');
            if (pathRoutes.Count() < 3)
            {
                await next.Invoke(context);
                return;
            }
            var isBasAPIController = pathRoutes[2].ToLower() == "bastask" || pathRoutes[2].ToLower() == "bastaskerror";
            if (pathRoutes[1].ToLower() == "api" && isBasAPIController)
            {
                if (context.Request.Query["apikey"].Count() != 0)
                {
                    if (workerServerDBService.CheckForAccess(context.Request.Query["apikey"]))
                    {
                        await next.Invoke(context);
                        return;
                    }
                }

                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("No access");
                return;
            }

            await next.Invoke(context);
            return;
        }
    }
}