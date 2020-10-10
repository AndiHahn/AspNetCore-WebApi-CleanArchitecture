using System;
using CleanArchitecture.Core.Queries;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.IntegrationTests.Helper
{
    public static class BudgetContextHelper
    {
        public static BudgetContext CreateInMemoryDataContext(Action<BudgetContext> setupCallback = null)
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<BudgetContext>()
                .UseSqlite(connection)
                .Options;

            // Create the schema in the database
            var context = new BudgetContext(options, new BillQueries());

            context.Database.EnsureCreated();

            // Invoke callback to enable Test to provide master data
            if (setupCallback != null)
            {
                setupCallback.Invoke(context);
                context.SaveChanges();
            }

            return context;
        }
    }
}
