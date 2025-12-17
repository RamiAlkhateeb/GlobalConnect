using GlobalConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalConnect.UnitTests.Helpers
{
    public static class DbContextFactory
    {
        public static GlobalConnectDbContext Create()
        {
            var options = new DbContextOptionsBuilder<GlobalConnectDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new GlobalConnectDbContext(options);
        }
    }
}
