using BASAccountManager.DB;
using BASAccountManager.DBServices;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using BASAccountManager;
using Hangfire;
using BASAccountManager.BackgroundTask;
using BASAccountManager.TaskManagers.InstManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IProxyDBService, ProxyDBService>();
builder.Services.AddTransient<IInstDBService, InstDBService>();
builder.Services.AddTransient<ISMSServiceDB, SMSDBService>();
builder.Services.AddTransient<ITaskDBService, TaskDBService>();
builder.Services.AddTransient<IWorkerTaskDBService, WorkerTaskDBService>();
builder.Services.AddTransient<IEmailDBService, EmailDBService>();
builder.Services.AddTransient<IBASExeptionDBService, BASExeptionDBService>();

builder.Services.AddTransient<HangFireTaskManager>();
builder.Services.AddTransient<AssignmentWriter>();
builder.Services.AddTransient<StatusMonitor>();
builder.Services.AddTransient<InstTaskManager>();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConf));
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
    .SetIsOriginAllowed(origin => true)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
}));

// DB Services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AMContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapControllers();
RecurringJob.AddOrUpdate<HangFireTaskManager>("TaskParser", (method) => method.TaskParser(), Cron.MinuteInterval(1));
RecurringJob.AddOrUpdate<HangFireTaskManager>("MonitorStatus", (method) => method.MonitorStatus(), Cron.MinuteInterval(1));
app.Run();