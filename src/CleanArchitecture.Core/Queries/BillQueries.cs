using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.Queries
{
    public class BillQueries : IBillQueries
    {
        private IBudgetContext context;

        public void SetBudgetContext(IBudgetContext context)
        {
            this.context = context;
        }

        public virtual IQueryable<BillEntity> WithRelations()
        {
            return context.Bill.Include(b => b.User).Include(b => b.Account).Include(b => b.BillCategory);
        }

        public virtual IQueryable<BillEntity> WithRelationsOrderedByDate()
        {
            return WithRelations().OrderByDescending(b => b.Date);
        }

        public virtual IQueryable<BillEntity> WithRelationsByAccountIdsOrdered(IEnumerable<int> accountIds)
        {
            IQueryable<BillEntity> query = WithRelationsOrderedByDate();
            if (accountIds != null)
            {
                query = query.Where(b => accountIds.Contains(b.AccountId));
            }

            return query;
        }

        public virtual IQueryable<BillEntity> WithCategoryByAccountIdBetweenDate(int accountId, DateTime fromDate, DateTime toDate)
        {
            return context.Bill.Include(b => b.BillCategory)
                               .Where(b => b.AccountId == accountId &&
                                           b.Date >= fromDate &&
                                           b.Date <= toDate);
        }
    }
}
