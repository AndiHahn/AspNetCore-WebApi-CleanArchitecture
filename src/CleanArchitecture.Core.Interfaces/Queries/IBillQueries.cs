using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Interfaces.Queries
{
    public interface IBillQueries
    {
        void SetBudgetContext(IBudgetContext context);
        IQueryable<BillEntity> WithRelations();
        IQueryable<BillEntity> WithRelationsOrderedByDate();
        IQueryable<BillEntity> WithRelationsByAccountIdsOrdered(IEnumerable<int> accountIds);
        IQueryable<BillEntity> WithCategoryByAccountIdBetweenDate(int accountId, DateTime fromDate, DateTime toDate);
    }
}
