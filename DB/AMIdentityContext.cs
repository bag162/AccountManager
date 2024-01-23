using BASAccountManager.DB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DB
{
    public class AMIdentityContext : IdentityDbContext<DBUser>
    {
        public AMIdentityContext(DbContextOptions<AMIdentityContext> options) : base(options){}
    }
}