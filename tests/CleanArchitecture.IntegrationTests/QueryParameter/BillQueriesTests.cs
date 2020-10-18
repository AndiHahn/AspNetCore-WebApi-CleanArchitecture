using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.IntegrationTests.Setup.Database;
using CleanArchitecture.Tests.Shared.Builder.Account;
using CleanArchitecture.Tests.Shared.Builder.Bill;
using CleanArchitecture.Tests.Shared.Builder.User;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CleanArchitecture.IntegrationTests.QueryParameter
{
    public class BillQueriesTests
    {
        [Fact]
        public async Task BillQueryWithRelations_ShouldReturnBillsWithRelations_Correctly()
        {
            var billEntity = CreateBasicBillEntity();
            var context = BudgetContext.CreateInMemoryDataContext(db =>
            {
                db.Bill.Add(billEntity);
            });

            var result = await context.BillQueries.WithRelations().ToListAsync();
            var billWithRelations = result.First();

            Assert.NotNull(billWithRelations.Account);
            Assert.NotNull(billWithRelations.BillCategory);
            Assert.NotNull(billWithRelations.User);
        }

        [Fact]
        public async Task BillQueryWithRelations_ShouldReturnEmptyResult_IfNoDataAvailable()
        {
            var context = BudgetContext.CreateInMemoryDataContext();

            var result = await context.BillQueries.WithRelations().ToListAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task BillQueryWithRelationsOrderedByData_ShouldBillsOrderedByDate_Correctly()
        {
            var billEntities = CreateBillEntitiesWithRandomDates();
            var context = BudgetContext.CreateInMemoryDataContext(db =>
            {
                foreach (var billEntity in billEntities)
                {
                    db.Bill.Add(billEntity);
                }
            });

            var result = await context.BillQueries.WithRelationsOrderedByDate().ToListAsync();

            DateTime previousDate = result.First().Date;
            foreach (var bill in result)
            {
                Assert.True(bill.Date <= previousDate);
                previousDate = bill.Date;
            }
        }

        private BillEntity CreateBasicBillEntity()
        {
            var accountEntity = new AccountEntityBuilder().Build();
            var userEntity = new UserEntityBuilder().Build();
            return new BillEntityBuilder().WithAccount(accountEntity).WithUser(userEntity).Build();
        }

        private BillEntity CreateBillEntityWithDate(DateTime date)
        {
            var accountEntity = new AccountEntityBuilder().Build();
            var userEntity = new UserEntityBuilder().Build();
            return new BillEntityBuilder().WithDate(date)
                        .WithAccount(accountEntity).WithUser(userEntity).Build();
        }

        private IList<BillEntity> CreateBillEntitiesWithRandomDates()
        {
            var faker = new Faker();

            return new List<BillEntity>()
            {
                CreateBillEntityWithDate(faker.Date.Past()),
                CreateBillEntityWithDate(faker.Date.Recent()),
                CreateBillEntityWithDate(faker.Date.Future()),
                CreateBillEntityWithDate(faker.Date.Future()),
                CreateBillEntityWithDate(faker.Date.Past())
            };
        }
    }
}
