using BASAccountManager.DB;
using BASAccountManager.DBServices;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BASAccountManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IProxyDBService, ProxyDBService>();
builder.Services.AddTransient<IFBDBService, FBDBService>();
builder.Services.AddTransient<ISMSServiceDB, SMSDBService>();

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
builder.Services.AddDbContext<AMContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();