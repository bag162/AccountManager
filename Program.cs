using BASAccountManager.DB;
using BASAccountManager.DBServices;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using BASAccountManager;
using Hangfire;
using BASAccountManager.BackgroundTask;
using BASAccountManager.TaskManagers.InstManager;
using BASAccountManager.DBServices.PostDBServices;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using BASAccountManager.DB.Models;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Hangfire.Dashboard.BasicAuthorization;
using BASAccountManager.DBServices.AdvertDBServices.Interfaces;
using BASAccountManager.DBServices.AdvertDBServices;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(opt =>
{
    opt.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    opt.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
});
// Add services to the container.
builder.Services.AddTransient<IProxyDBService, ProxyDBService>();
builder.Services.AddTransient<IInstDBService, InstDBService>();
builder.Services.AddTransient<ISMSServiceDB, SMSDBService>();
builder.Services.AddTransient<ITaskDBService, TaskDBService>();
builder.Services.AddTransient<IWorkerTaskDBService, WorkerTaskDBService>();
builder.Services.AddTransient<IEmailDBService, EmailDBService>();
builder.Services.AddTransient<IBASExeptionDBService, BASExeptionDBService>();
builder.Services.AddTransient<IFollowDBService, FollowDBService>();
builder.Services.AddTransient<ICommentDBService, CommentDBService>();
builder.Services.AddTransient<IFillingDataDBService, FillingDataDBService>();
builder.Services.AddTransient<IClonGroupDBService, ClonGroupDBService>();
builder.Services.AddTransient<IClonDBService, ClonDBService>();
builder.Services.AddTransient<ISchedulerTaskDBService, SchedulerTaskDBService>();

builder.Services.AddTransient<IPostDBService, PostDBService>();
builder.Services.AddTransient<IPostCommentDBService, PostCommentDBService>();
builder.Services.AddTransient<IPostGroupDBService, PostGroupDBService>();
builder.Services.AddTransient<IInstPostDBService, InstPostDBService>();
builder.Services.AddTransient<IPostCommentGroupDBService, PostCommentGroupDBService>();
builder.Services.AddTransient<IPostLikeDBService, PostLikeDBService>();

builder.Services.AddTransient<IAdvertAccountDBService, AdvertAccountDBService>();
builder.Services.AddTransient<IAdvertAccountGroupDBService, AdvertAccountGroupDBService>();
builder.Services.AddTransient<IAdvertPostGroupDBService, AdvertPostGroupDBService>();
builder.Services.AddTransient<IAdvertPostDBService, AdvertPostDBService>();

builder.Services.AddTransient<HangFireTaskManager>();
builder.Services.AddTransient<AssignmentWriter>();
builder.Services.AddTransient<StatusMonitor>();
builder.Services.AddTransient<InstTaskManager>();

builder.Services.AddMemoryCache();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer(opt =>
{
    opt.StopTimeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConf));

builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
{
    builder
    .WithOrigins("http://localhost:80")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

// DB Services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AMContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
builder.Services.AddDbContext<AMIdentityContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/auth/login";
    opt.LogoutPath = "/auth/logut";
    opt.AccessDeniedPath = "/";
    opt.ExpireTimeSpan = TimeSpan.FromDays(180);
});

builder.Services.AddIdentity<DBUser, IdentityRole>(config =>
{
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequiredLength = 1;
    config.Password.RequireUppercase = false;
    config.Password.RequireDigit = false;
    config.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<AMIdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot";
});

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthFilter() }
});

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
    if (app.Environment.IsDevelopment())
    {
        spa.UseAngularCliServer(npmScript: "start");
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});

RecurringJob.AddOrUpdate<HangFireTaskManager>("TaskParser", (method) => method.TaskParser(), "*/1 * * * * *");
RecurringJob.AddOrUpdate<HangFireTaskManager>("PostParser", (method) => method.PostParser(), "*/1 * * * * *");
RecurringJob.AddOrUpdate<HangFireTaskManager>("CommentParser", (method) => method.CommentParser(), "*/1 * * * * *");
RecurringJob.AddOrUpdate<HangFireTaskManager>("LikesParser", (method) => method.LikesParser(), "*/1 * * * * *");
RecurringJob.AddOrUpdate<HangFireTaskManager>("MonitorStatus", (method) => method.MonitorStatus(), "*/1 * * * * *");
RecurringJob.AddOrUpdate<HangFireTaskManager>("ParseSchedulerTask", (method) => method.ParseSchedulerTask(), "*/1 * * * * *");

// Init Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(rolesManager);
    }
    catch (Exception ex)
    {
    }
}

app.Run();