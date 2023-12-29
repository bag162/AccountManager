using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DB
{
    public class AMContext : DbContext
    {
        public AMContext(DbContextOptions<AMContext> options) : base(options){}

        public DbSet<DBBASExeption> BASExeption { get; set; }

        public DbSet<DBInstAccountGroup> InstAccountGroup { get; set; }
        public DbSet<DBInstagramAccount> InstAccount { get; set; }
        public DbSet<DBTask> Task { get; set; }
        public DbSet<DBWorkerTask> WorkerTask { get; set; }

        public DbSet<DBEmail> Email { get; set; }
        public DbSet<DBSMSActivation> SMSActivation { get; set; }
        public DbSet<DBProxy> Proxy { get; set; }
        public DbSet<DBProxyGroup> ProxyGroup { get; set; }

        public DbSet<DBPost> Post { get; set; }
        public DbSet<DBPostLike> PostLike { get; set; }
        public DbSet<DBPostComment> PostComment { get; set; }
        public DbSet<DBPostGroup> PostGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBProxyGroup>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<DBInstAccountGroup>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<DBPostGroup>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<DBPost>().HasIndex(x => x.PostURI).IsUnique();
        }
    }
}