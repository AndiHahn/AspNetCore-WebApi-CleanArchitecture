using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CleanArchitecture.Core.Interfaces.Services.Bill.Models;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.IntegrationTests.Setup.Database;
using CleanArchitecture.Tests.Shared.Builder.Account;
using CleanArchitecture.Tests.Shared.Builder.Bill;
using CleanArchitecture.Tests.Shared.Builder.User;
using Xunit;

namespace CleanArchitecture.IntegrationTests.QueryParameter
{
    public class BillQueriesTests
    {
        [Fact]
        public async Task Query_ShouldReturn_CorrectNumberOfBills()
        {
            var billEntity1 = CreateBasicBillEntity();
            var billEntity2 = CreateBasicBillEntity();
            var context = BudgetContext.CreateInMemoryDataContext(db =>
            {
                db.Bill.Add(billEntity1);
                db.Bill.Add(billEntity2);
            });

            var queryParameter = new BillQueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            };

            var result = await context.BillQueries.QueryAsync(queryParameter);

            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Result.Count());
        }

        [Fact]
        public async Task Query_ShouldReturnEmptyResult_IfNoDataAvailable()
        {
            var context = BudgetContext.CreateInMemoryDataContext();

            var queryParameter = new BillQueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            };

            var result = await context.BillQueries.QueryAsync(queryParameter);
            Assert.Equal(0, result.TotalCount);
            Assert.Empty(result.Result);
        }

        [Fact]
        public async Task Query_ShouldReturnBills_OrderedByDateDescending()
        {
            var billEntities = CreateBillEntitiesWithRandomDates();
            var context = BudgetContext.CreateInMemoryDataContext(db =>
            {
                foreach (var billEntity in billEntities)
                {
                    db.Bill.Add(billEntity);
                }
            });

            var queryParameter = new BillQueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            };

            var result = await context.BillQueries.QueryAsync(queryParameter);

            DateTime previousDate = result.Result.First().Date;
            foreach (var bill in result.Result)
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

            return new List<BillEntity>
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
