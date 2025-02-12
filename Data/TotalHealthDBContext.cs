using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TotalHealth.Models;

namespace TotalHealth.Data
{
    public class TotalHealthDBContext : IdentityDbContext
    {
        public TotalHealthDBContext(DbContextOptions<TotalHealthDBContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        }
    }
}
