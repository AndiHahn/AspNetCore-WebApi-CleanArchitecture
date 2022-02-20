using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shared.Tests.Builder;
using CleanArchitecture.Shopping.Application.Bill;
using CleanArchitecture.Shopping.Core.Bill;
using CleanArchitecture.Web.Api.FunctionalTests.Extensions;
using CleanArchitecture.Web.Api.FunctionalTests.Fixture;
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
            var result = await response.ResolveAsync<PagedResult<BillDto>>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Single(result.Result);
            Assert.Equal(1, result.TotalCount);
            AssertBillDtoEqualModel(bill, result.Result.First());
        }

        [Fact]
        public async Task GetBills_ShouldReturnEmptyResult_IfNoDataAvailableInDatabase()
        {
            // Arrange
            var client = await apiFunctionalTestFixture.CreateAuthorizedClientAsync();

            apiFunctionalTestFixture.SetupDatabase();

            // Act
            var response = await client.GetAsync("/api/bill");
            var result = await response.ResolveAsync<PagedResult<BillDto>>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Empty(result.Result);
            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async Task GetBills_ShouldReturnUnauthorized_IfRequestDoesNotContainBearerToken()
        {
            // Arrange
            var client = apiFunctionalTestFixture.CreateClient();

            // Act
            var response = await client.GetAsync("/api/bill");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
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
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            AssertBillDtoEqualModel(bill, result);
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

        private void AssertBillDtoEqualModel(Bill model, BillDto dto)
        {
            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.ShopName, dto.ShopName);
            Assert.Equal(model.Price, dto.Price);
            Assert.Equal(model.Date, dto.Date);
            Assert.Equal(model.Notes, dto.Notes);
            Assert.Equal(model.Category, dto.Category.FromDto());
        }
    }
}
