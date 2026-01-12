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
            
        }
    }
}
