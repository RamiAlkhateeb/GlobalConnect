using GlobalConnect.Domain.Models;
using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlobalConnect.UnitTests.Helpers
{
    public static class DatabaseHelper
    {
        public static GlobalConnectDbContext CreateInMemoryDatabase(string dbName)
        {
            var options = new DbContextOptionsBuilder<GlobalConnectDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new GlobalConnectDbContext(options);
            return context;
        }

        public static void SeedProvider(GlobalConnectDbContext context, int id, string name, string specialty)
        {
            context.Providers.Add(new Provider
            {
                UserId = id,
                Name = name,
                Specialty = specialty,
                Bio = "Test Bio",
                Nationality = "Test Country",
                GoogleBookingUrl = "http://test.com",
                User = new User { Id = id, Email = $"test{id}@test.com" }
            });
            context.SaveChanges();
        }
    }
}
