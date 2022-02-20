using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;

namespace CleanArchitecture.Shopping.Infrastructure.UnitOfWork
{
    internal class UnitOfWork : IDisposable, IUnitOfWork
    {
        public UnitOfWork(ShoppingDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            this.BankAccountRepository = new BankAccountRepository(context);
            this.BillRepository = new BillRepository(context);
            this.UserRepository = new UserRepository(context);
        }

        private bool disposed = false;
        private readonly ShoppingDbContext context;

        public IBankAccountRepository BankAccountRepository { get; }
        public IBillRepository BillRepository { get; }
        public IUserRepository UserRepository { get; }

        public Task CommitAsync(CancellationToken cancellationToken) => this.context.SaveChangesAsync(cancellationToken);

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
