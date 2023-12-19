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
        public DbSet<DBFBAccountGroup> FBAccountGroup { get; set; }

        public DbSet<DBProxy> Proxy { get; set; }

        public DbSet<DBFacebookAccount> FBAccount { get; set; }

        public DbSet<DBSMSActivation> SMSActivation { get; set; }

        public DbSet<DBTask> Task { get; set; }
        public DbSet<DBWorkerTask> WorkerTask { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBProxyGroup>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<DBFBAccountGroup>().HasIndex(x => x.Name).IsUnique();
        }
    }
}