using GlobalConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalConnect.Infrastructure.Data
{
    public class GlobalConnectDbContext : DbContext
    {
        public GlobalConnectDbContext(DbContextOptions<GlobalConnectDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -----------------------------
            // 1. User Configuration
            // -----------------------------
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id); // Int is PK


            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

          

        }

        // The Tables in your Database
        public DbSet<User> Users { get; set; }
    }
}
