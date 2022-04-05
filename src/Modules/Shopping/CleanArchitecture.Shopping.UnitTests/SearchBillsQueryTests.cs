using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using CleanArchitecture.Shopping.Application.Bill.Queries;
using CleanArchitecture.Shopping.Application.Bill.Queries.SearchBills;
using CleanArchitecture.Shopping.Application.Mapping;
using CleanArchitecture.Shopping.Core;
using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.UnitTests.Builder;
using CleanArchitecture.Shopping.UnitTests.Setup;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Shopping.UnitTests
{
    public class SearchBillsQueryTests
    {
        private readonly User user = new UserBuilder().Build();

        public SearchBillsQueryTests()
        {
        }

        [Fact]
        public async Task SearchBillsQuery_ShouldReturn_CorrectNumberOfBills()
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
            var result = await sut.Handle(new SearchBillsQuery(user.Id), default);

            // Assert
            result.TotalCount.Should().Be(2);
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task SearchBillsQuery_ShouldReturnEmptyResult_IfNoDataAvailable()
        {
            // Arrange
            var context = BudgetContext.CreateInMemoryDataContext();

            var sut = SetupSystemUnderTest(context);

            // Act
            var result = await sut.Handle(new SearchBillsQuery(user.Id), default);

            // Assert
            result.TotalCount.Should().Be(0);
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchBillsQuery_ShouldReturnBills_OrderedByDateDescending()
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
            var result = await sut.Handle(new SearchBillsQuery(user.Id), default);

            // Assert
            result.Value.Should().BeInDescendingOrder(dto => dto.Date);
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

        private SearchBillsQueryHandler SetupSystemUnderTest(
            IShoppingDbContext context)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            return new SearchBillsQueryHandler(context, configuration.CreateMapper());
        }
    }
}
