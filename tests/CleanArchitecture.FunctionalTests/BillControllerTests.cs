using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Core.Models.Domain.Bill;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.FunctionalTests.Extensions;
using CleanArchitecture.FunctionalTests.Fixture;
using CleanArchitecture.Tests.Shared.Builder.Account;
using CleanArchitecture.Tests.Shared.Builder.Bill;
using CleanArchitecture.Tests.Shared.Builder.User;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CleanArchitecture.FunctionalTests
{
    public class BillControllerTests : IClassFixture<ApiFunctionalTestFixture>
    {
        private readonly ApiFunctionalTestFixture apiFunctionalTestFixture;
        private readonly HttpClient authorizedClient;

        public BillControllerTests(ApiFunctionalTestFixture apiFunctionalTestFixture)
        {
            this.apiFunctionalTestFixture = apiFunctionalTestFixture;
            authorizedClient = apiFunctionalTestFixture.CreateAuthorizedClient();
        }

        [Fact]
        public async Task GetBills_ShouldReturnBills_Correctly()
        {
            var userEntity = new UserEntityBuilder(apiFunctionalTestFixture.UserId).Build();
            var accountEntity = new AccountEntityBuilder().Build();
            var billEntity = new BillEntityBuilder().WithAccount(accountEntity).CreatedByUser(userEntity).Build();
            apiFunctionalTestFixture.SetupDatabase(db =>
            {
                db.Bill.Add(billEntity);
            });

            var response = await authorizedClient.GetAsync("/api/bill");
            var result = await response.ResolveAsync<PagedResult<BillModel>>();

            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Single(result.Result);
            Assert.Equal(1, result.TotalCount);
            AssertBillEntityEqualModel(billEntity, result.Result.First());
        }

        [Fact]
        public async Task GetBills_ShouldReturnEmptyResult_IfNoDataAvailableInDatabase()
        {
            apiFunctionalTestFixture.SetupDatabase();

            var response = await authorizedClient.GetAsync("/api/bill");
            var result = await response.ResolveAsync<PagedResult<BillModel>>();

            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Empty(result.Result);
            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async Task GetBills_ShouldReturnUnauthorized_IfRequestDoesNotContainBearerToken()
        {
            var client = apiFunctionalTestFixture.CreateClient();
            var response = await client.GetAsync("/api/bill");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetBill_ShouldReturnBill_Correctly()
        {
            var userEntity = new UserEntityBuilder(apiFunctionalTestFixture.UserId).Build();
            var accountEntity = new AccountEntityBuilder().Build();
            var billEntity = new BillEntityBuilder().WithAccount(accountEntity).CreatedByUser(userEntity).Build();
            apiFunctionalTestFixture.SetupDatabase(db =>
            {
                db.Bill.Add(billEntity);
            });

            var response = await authorizedClient.GetAsync($"/api/bill/{billEntity.Id}");
            var result = await response.ResolveAsync<BillModel>();

            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            AssertBillEntityEqualModel(billEntity, result);
        }

        [Fact]
        public async Task GetBill_ShouldReturnNotFound_IfIdIsNotAvailable()
        {
            apiFunctionalTestFixture.SetupDatabase();
            var response = await authorizedClient.GetAsync($"/api/bill/{Guid.NewGuid()}");
            var result = await response.ResolveAsync<ProblemDetails>();

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(404, result.Status);
        }

        private void AssertBillEntityEqualModel(BillEntity entity, BillModel model)
        {
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.ShopName, model.ShopName);
            Assert.Equal(entity.Price, model.Price);
            Assert.Equal(entity.Date, model.Date);
            Assert.Equal(entity.Notes, model.Notes);
            Assert.Equal(entity.Category, model.Category);
        }
    }
}
