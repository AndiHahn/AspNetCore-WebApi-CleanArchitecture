using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CleanArchitecture.Core.QueryParameter.Models;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.FunctionalTests.Helper;
using CleanArchitecture.Services.Bill.Models;
using CleanArchitecture.Tests.Shared.Builder.Account;
using CleanArchitecture.Tests.Shared.Builder.Bill;
using CleanArchitecture.Tests.Shared.Builder.User;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CleanArchitecture.FunctionalTests
{
    public class BillControllerTests : IClassFixture<ApiTestFixture>
    {
        private readonly ApiTestFixture apiTestFixture;
        private readonly HttpClient authorizedClient;

        public BillControllerTests(ApiTestFixture apiTestFixture)
        {
            this.apiTestFixture = apiTestFixture;
            authorizedClient = apiTestFixture.CreateClient();
            string token = ApiTokenHelper.GetToken();
            authorizedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task GetBills_ShouldReturnBills_Correctly()
        {
            var userEntity = new UserEntityBuilder().Build();
            var accountEntity = new AccountEntityBuilder().Build();
            var billEntity = new BillEntityBuilder().WithAccount(accountEntity).WithUser(userEntity).Build();
            apiTestFixture.SetupDatabase(db =>
            {
                db.Bill.Add(billEntity);
            });

            var response = await authorizedClient.GetAsync("/api/bill");
            var result = await ApiResultHelper.GetModelFromResponseAsync<PagedResult<BillModel>>(response);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Single(result.Result);
            Assert.Equal(1, result.TotalCount);
            AssertBillEntityEqualModel(billEntity, result.Result.First());
        }

        [Fact]
        public async Task GetBills_ShouldReturnEmptyResult_IfNoDataAvailableInDatabase()
        {
            apiTestFixture.SetupDatabase();

            var response = await authorizedClient.GetAsync("/api/bill");
            var result = await ApiResultHelper.GetModelFromResponseAsync<PagedResult<BillModel>>(response);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Empty(result.Result);
            Assert.Equal(0, result.TotalCount);
        }

        [Fact]
        public async Task GetBills_ShouldReturnUnauthorized_IfRequestDoesNotContainBearerToken()
        {
            var client = apiTestFixture.CreateClient();
            var response = await client.GetAsync("/api/bill");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetBill_ShouldReturnBill_Correctly()
        {
            var userEntity = new UserEntityBuilder().Build();
            var accountEntity = new AccountEntityBuilder().Build();
            var billEntity = new BillEntityBuilder().WithAccount(accountEntity).WithUser(userEntity).Build();
            apiTestFixture.SetupDatabase(db =>
            {
                db.Bill.Add(billEntity);
            });

            var response = await authorizedClient.GetAsync($"/api/bill/{billEntity.Id}");
            var result = await ApiResultHelper.GetModelFromResponseAsync<BillModel>(response);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            AssertBillEntityEqualModel(billEntity, result);
        }

        [Fact]
        public async Task GetBill_ShouldReturnNotFound_IfIdIsNotAvailable()
        {
            apiTestFixture.SetupDatabase();
            var response = await authorizedClient.GetAsync($"/api/bill/4");
            var result = await ApiResultHelper.GetModelFromResponseAsync<ProblemDetails>(response);

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
            Assert.Equal(entity.BillCategory.Name, model.Category);
        }
    }
}
