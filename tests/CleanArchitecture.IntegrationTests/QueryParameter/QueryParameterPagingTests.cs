using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.QueryParameter;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.IntegrationTests.QueryParameter.TestData;
using CleanArchitecture.IntegrationTests.Setup.Database;
using CleanArchitecture.Tests.Shared.Builder.Account;
using CleanArchitecture.Tests.Shared.Builder.Bill;
using CleanArchitecture.Tests.Shared.Builder.User;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CleanArchitecture.IntegrationTests.QueryParameter
{
    public class QueryParameterPagingTests
    {
        [Fact]
        public async Task ApplyPaging_ShouldReturnPagedResult_Correctly()
        {
            var billEntities = CreateBillEntities();
            var context = BudgetContext.CreateInMemoryDataContext(db =>
            {
                foreach (var bill in billEntities)
                {
                    db.Bill.Add(bill);
                }
            });

            var pagedResult = await context.Bill.ApplyPaging(new TestPagingParameter(2, 0)).ToListAsync();
            Assert.Equal(2, pagedResult.Count());
        }

        [Fact]
        public async Task ApplyPaging_ShouldThrowException_IfPagingParametersAreNull()
        {
            var context = BudgetContext.CreateInMemoryDataContext();
            await Assert.ThrowsAsync<BadRequestException>(() =>
                            context.Bill.ApplyPaging(null).ToListAsync());
        }

        [Fact]
        public async Task ApplyPaging_ShouldThrowException_IfPageSizeIsZero()
        {
            var billEntities = CreateBillEntities();
            var context = BudgetContext.CreateInMemoryDataContext(db =>
            {
                foreach (var bill in billEntities)
                {
                    db.Bill.Add(bill);
                }
            });

            await Assert.ThrowsAsync<BadRequestException>(() =>
                            context.Bill.ApplyPaging(new TestPagingParameter(0, 0)).ToListAsync());
        }

        private IList<BillEntity> CreateBillEntities()
        {
            return new List<BillEntity>()
            {
                CreateBasicBillEntity(),
                CreateBasicBillEntity(),
                CreateBasicBillEntity(),
                CreateBasicBillEntity(),
                CreateBasicBillEntity()
            };
        }

        private BillEntity CreateBasicBillEntity()
        {
            var accountEntity = new AccountEntityBuilder().Build();
            var userEntity = new UserEntityBuilder().Build();
            return new BillEntityBuilder().WithAccount(accountEntity).WithUser(userEntity).Build();
        }
    }
}
