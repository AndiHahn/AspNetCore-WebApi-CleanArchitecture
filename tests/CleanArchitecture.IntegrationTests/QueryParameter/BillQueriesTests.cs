using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Interfaces.SqlQueries;
using CleanArchitecture.Core.Models.Domain.Bill;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.SqlQueries;
using CleanArchitecture.IntegrationTests.Setup.Database;
using CleanArchitecture.Tests.Shared.Builder.Account;
using CleanArchitecture.Tests.Shared.Builder.Bill;
using CleanArchitecture.Tests.Shared.Builder.User;
using Xunit;

namespace CleanArchitecture.IntegrationTests.QueryParameter
{
    public class BillQueriesTests
    {
        private readonly UserEntity user = new UserEntityBuilder().Build();

        public BillQueriesTests()
        {
        }

        [Fact]
        public async Task Query_ShouldReturn_CorrectNumberOfBills()
        {
            // Arrange
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

            var sut = SetupSystemUnderTest(context);

            // Act
            var result = await sut.QueryAsync(queryParameter, user.Id);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Result.Count());
        }

        [Fact]
        public async Task Query_ShouldReturnEmptyResult_IfNoDataAvailable()
        {
            // Arrange
            var context = BudgetContext.CreateInMemoryDataContext();

            var queryParameter = new BillQueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            };

            var sut = SetupSystemUnderTest(context);

            // Act
            var result = await sut.QueryAsync(queryParameter, user.Id);

            // Assert
            Assert.Equal(0, result.TotalCount);
            Assert.Empty(result.Result);
        }

        [Fact]
        public async Task Query_ShouldReturnBills_OrderedByDateDescending()
        {
            // Arrange
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

            var sut = SetupSystemUnderTest(context);

            // Act
            var result = await sut.QueryAsync(queryParameter, user.Id);

            // Assert
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
            return new BillEntityBuilder().WithAccount(accountEntity).CreatedByUser(user).Build();
        }

        private BillEntity CreateBillEntityWithDate(DateTime date)
        {
            var accountEntity = new AccountEntityBuilder().Build();
            return new BillEntityBuilder().WithDate(date)
                        .WithAccount(accountEntity).CreatedByUser(user).Build();
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

        private IBillQueries SetupSystemUnderTest(IBudgetContext context)
        {
            var sut = new BillQueries();
            sut.SetBudgetContext(context);
            return sut;
        }
    }
}
