using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CleanArchitecture.Shopping.Core;
using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;
using CleanArchitecture.Shopping.IntegrationTests.Setup;
using CleanArchitecture.Shopping.UnitTests.Builder;
using Xunit;

namespace CleanArchitecture.Shopping.IntegrationTests.Infrastructure
{
    public class BillRepositoryTests
    {
        private readonly User user = new UserBuilder().Build();

        public BillRepositoryTests()
        {
        }

        [Fact]
        public async Task SearchBills_ShouldReturn_CorrectNumberOfBills()
        {
            // Arrange
            var billEntity1 = CreateBasicBill();
            var billEntity2 = CreateBasicBill();
            var context = BudgetContext.CreateInMemoryDataContext(db =>
            {
                db.Bill.Add(billEntity1);
                db.Bill.Add(billEntity2);
            });

            var sut = SetupSystemUnderTest(context);

            // Act
            var result = await sut.SearchBillsAsync(user.Id);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task SearchBills_ShouldReturnEmptyResult_IfNoDataAvailable()
        {
            // Arrange
            var context = BudgetContext.CreateInMemoryDataContext();

            var sut = SetupSystemUnderTest(context);

            // Act
            var result = await sut.SearchBillsAsync(user.Id);

            // Assert
            Assert.Equal(0, result.TotalCount);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task SearchBills_ShouldReturnBills_OrderedByDateDescending()
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

            var sut = SetupSystemUnderTest(context);

            // Act
            var result = await sut.SearchBillsAsync(user.Id);

            // Assert
            DateTime previousDate = result.Value.First().Date;
            foreach (var bill in result.Value)
            {
                Assert.True(bill.Date <= previousDate);
                previousDate = bill.Date;
            }
        }

        private Bill CreateBasicBill()
        {
            var accountEntity = new AccountBuilder().Owner(this.user).Build();
            return new BillBuilder().WithAccount(accountEntity).CreatedByUser(this.user).Build();
        }

        private Bill CreateBillEntityWithDate(DateTime date)
        {
            var accountEntity = new AccountBuilder().Owner(this.user).Build();
            return new BillBuilder().WithDate(date)
                        .WithAccount(accountEntity).CreatedByUser(this.user).Build();
        }

        private IList<Bill> CreateBillEntitiesWithRandomDates()
        {
            var faker = new Faker();

            return new List<Bill>
            {
                CreateBillEntityWithDate(faker.Date.Past()),
                CreateBillEntityWithDate(faker.Date.Recent()),
                CreateBillEntityWithDate(faker.Date.Future()),
                CreateBillEntityWithDate(faker.Date.Future()),
                CreateBillEntityWithDate(faker.Date.Past())
            };
        }

        private IBillRepository SetupSystemUnderTest(
            ShoppingDbContext context)
        {
            return new Shopping.Infrastructure.Repositories.BillRepository(context);
        }
    }
}
