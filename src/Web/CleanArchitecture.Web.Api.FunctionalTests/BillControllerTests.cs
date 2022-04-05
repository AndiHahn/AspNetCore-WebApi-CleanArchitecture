using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Application.Bill;
using CleanArchitecture.Shopping.Core;
using CleanArchitecture.Shopping.UnitTests.Builder;
using CleanArchitecture.Web.Api.FunctionalTests.Extensions;
using CleanArchitecture.Web.Api.FunctionalTests.Fixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CleanArchitecture.Web.Api.FunctionalTests
{
    public class BillControllerTests : IClassFixture<ApiFunctionalTestFixture>
    {
        private readonly ApiFunctionalTestFixture apiFunctionalTestFixture;

        public BillControllerTests(ApiFunctionalTestFixture apiFunctionalTestFixture)
        {
            this.apiFunctionalTestFixture = apiFunctionalTestFixture;
        }

        [Fact]
        public async Task GetBills_ShouldReturnBills_Correctly()
        {
            // Arrange
            var client = await apiFunctionalTestFixture.CreateAuthorizedClientAsync();

            var user = new UserBuilder().Id(apiFunctionalTestFixture.UserId).Build();
            var account = new AccountBuilder().Owner(user).Build();
            var bill = new BillBuilder()
                .WithAccount(account)
                .CreatedByUser(user).Build();
            apiFunctionalTestFixture.SetupDatabase(db =>
            {
                db.Bill.Add(bill);
            });

            // Act
            var response = await client.GetAsync("/api/bill");
            var result = await response.ResolveAsync<PagedResultDto<BillDto>>();

            // Assert
            var expectedDto = ToBillDto(bill, account.Id, user.Id);

            response.EnsureSuccessStatusCode();
            result.Should().NotBeNull();
            result.Values.Should().HaveCount(1);
            result.Values.First().Should().BeEquivalentTo(expectedDto, options => options.Excluding(dto => dto.Version));
        }

        [Fact]
        public async Task GetBills_ShouldReturnEmptyResult_IfNoDataAvailableInDatabase()
        {
            // Arrange
            var client = await apiFunctionalTestFixture.CreateAuthorizedClientAsync();

            apiFunctionalTestFixture.SetupDatabase();

            // Act
            var response = await client.GetAsync("/api/bill");
            var result = await response.ResolveAsync<PagedResultDto<BillDto>>();

            // Assert
            response.EnsureSuccessStatusCode();
            result.Should().NotBeNull();
            result.Values.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetBills_ShouldReturnUnauthorized_IfRequestDoesNotContainBearerToken()
        {
            // Arrange
            var client = apiFunctionalTestFixture.CreateClient();

            // Act
            var response = await client.GetAsync("/api/bill");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetBill_ShouldReturnBill_Correctly()
        {
            // Arrange
            var client = await apiFunctionalTestFixture.CreateAuthorizedClientAsync();

            var user = new UserBuilder().Id(apiFunctionalTestFixture.UserId).Build();
            var account = new AccountBuilder().Owner(user).Build();
            var bill = new BillBuilder().WithAccount(account).CreatedByUser(user).Build();
            apiFunctionalTestFixture.SetupDatabase(db =>
            {
                db.Bill.Add(bill);
            });

            // Act
            var response = await client.GetAsync($"/api/bill/{bill.Id}");
            var result = await response.ResolveAsync<BillDto>();

            // Assert
            var expectedDto = ToBillDto(bill, account.Id, user.Id);

            response.EnsureSuccessStatusCode();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto, options => options.Excluding(dto => dto.Version));
        }

        [Fact]
        public async Task GetBill_ShouldReturnNotFound_IfIdIsNotAvailable()
        {
            // Arrange
            var client = await apiFunctionalTestFixture.CreateAuthorizedClientAsync();
            apiFunctionalTestFixture.SetupDatabase();

            // Act
            var response = await client.GetAsync($"/api/bill/{Guid.NewGuid()}");
            var result = await response.ResolveAsync<ProblemDetails>();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(404, result.Status);
        }

        private static BillDto ToBillDto(Bill bill, Guid accountId, Guid userId)
            => new BillDto
            {
                Id = bill.Id,
                ShopName = bill.ShopName,
                Category = bill.Category.ToDto(),
                Date = bill.Date,
                Notes = bill.Notes,
                Price = bill.Price,
                BankAccountId = accountId,
                CreatedByUserId = userId,
            };
    }
}
