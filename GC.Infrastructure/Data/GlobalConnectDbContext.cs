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

            // -----------------------------
            // 1. User Configuration
            // -----------------------------
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id); // Int is PK


            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // -----------------------------
            // 2. One-to-One (User <-> ProviderProfile)
            // -----------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.ProviderProfile)
                .WithOne(p => p.User)
                .HasForeignKey<Provider>(p => p.UserId) // Int FK
                .OnDelete(DeleteBehavior.Cascade);

            // -----------------------------
            // 3. Many-to-Many (Provider <-> Language)
            // -----------------------------
            modelBuilder.Entity<ProviderLanguage>()
                .HasKey(pl => new { pl.ProviderId, pl.LanguageId });

            // Seed Languages (Initial Data)
            modelBuilder.Entity<Language>().HasData(
                new Language { LanguageId = 1, Name = "English" },
                new Language { LanguageId = 2, Name = "Arabic" },
                new Language { LanguageId = 3, Name = "French" },
                new Language { LanguageId = 4, Name = "Spanish" },
                new Language { LanguageId = 5, Name = "German" }
                );

        }

        // The Tables in your Database
        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderLanguage> ProviderLanguages { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
