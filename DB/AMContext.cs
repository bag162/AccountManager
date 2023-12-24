using BASAccountManager.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DB
{
    public class AMContext : DbContext
    {
        ILogger<AMContext> logger;

        public AMContext(DbContextOptions<AMContext> options, ILogger<AMContext> logger) : base(options)
        {
            this.logger = logger;
            Database.EnsureCreated();
        }

        public DbSet<DBProxyGroup> ProxyGroup { get; set; }
        public DbSet<DBInstAccountGroup> InstAccountGroup { get; set; }

        public DbSet<DBProxy> Proxy { get; set; }

        public DbSet<DBInstagramAccount> InstAccount { get; set; }

        public DbSet<DBSMSActivation> SMSActivation { get; set; }

        public DbSet<DBTask> Task { get; set; }
        public DbSet<DBWorkerTask> WorkerTask { get; set; }
        
        public DbSet<DBEmail> Email { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBProxyGroup>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<DBInstAccountGroup>().HasIndex(x => x.Name).IsUnique();
        }
    }
}