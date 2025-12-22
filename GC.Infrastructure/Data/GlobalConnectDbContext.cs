using Domain.Models;
using GlobalConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // === Configuration 1: One-to-One Relationship (User <-> Provider) ===
            // A User has one Provider Profile. A Provider Profile belongs to one User.
            modelBuilder.Entity<Provider>()
                .HasOne(p => p.User)
                .WithOne(u => u.ProviderProfile)
                .HasForeignKey<Provider>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // If User is deleted, delete Profile

            // === Configuration 2: Money Precision ===
            // SQL Server needs to know the precision for the 'money' or 'decimal' type
            modelBuilder.Entity<Provider>()
                .Property(p => p.HourlyRateUSD)
                .HasColumnType("decimal(18,2)");

            // === Configuration 3: Composite Keys or Indexes (if needed) ===
            // Example: Fast lookup for slots by Date + Provider
            //modelBuilder.Entity<GeneratedSlot>()
             //   .HasIndex(s => new { s.ProviderId, s.SlotStartUTC });

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Id)
            //    .UseIdentityColumn();
        }

        // The Tables in your Database
        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderLanguage> ProviderLanguages { get; set; }
        //public DbSet<WorkingHour> WorkingHours { get; set; }
        //public DbSet<GeneratedSlot> GeneratedSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
