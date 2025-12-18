using GlobalConnect.Infrastructure.Data;
using Microsoft.Data.Sqlite;
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
            // 1. Create a connection to a nameless in-memory database
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<GlobalConnectDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new GlobalConnectDbContext(options);

            // 2. Since SQLite starts empty, we must manually create the tables
            context.Database.EnsureCreated();

            return new GlobalConnectDbContext(options);
        }
    }
}
