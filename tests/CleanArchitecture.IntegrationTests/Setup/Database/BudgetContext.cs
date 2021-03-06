﻿using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.IntegrationTests.Setup.Database
{
    public static class BudgetContext
    {
        public static CleanArchitecture.Infrastructure.Database.Budget.BudgetContext CreateInMemoryDataContext(Action<CleanArchitecture.Infrastructure.Database.Budget.BudgetContext> setupCallback = null)
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<CleanArchitecture.Infrastructure.Database.Budget.BudgetContext>()
                .UseSqlite(connection)
                .Options;

            // Create the schema in the database
            var context = new CleanArchitecture.Infrastructure.Database.Budget.BudgetContext(
                options);

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
