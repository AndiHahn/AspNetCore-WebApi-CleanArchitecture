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
        }

        private bool disposed = false;
        private readonly ShoppingDbContext context;
        private readonly object lockObject = new();

        private IBankAccountRepository bankAccountRepository;
        private IBillRepository billRepository;
        private IUserRepository userRepository;

        public IBankAccountRepository BankAccountRepository
        {
            get
            {
                if (this.bankAccountRepository is not null)
                {
                    return this.bankAccountRepository;
                }

                lock (lockObject)
                {
                    this.bankAccountRepository ??= new BankAccountRepository(context);
                }

                return this.bankAccountRepository;
            }
        }

        public IBillRepository BillRepository
        {
            get
            {
                if (this.billRepository is not null)
                {
                    return this.billRepository;
                }

                lock (lockObject)
                {
                    this.billRepository ??= new BillRepository(context);
                }

                return this.billRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (this.userRepository is not null)
                {
                    return this.userRepository;
                }

                lock (lockObject)
                {
                    this.userRepository ??= new UserRepository(context);
                }

                return this.userRepository;
            }
        }

        public Task CommitAsync(CancellationToken cancellationToken) => this.context.SaveChangesAsync(cancellationToken);

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                context.Dispose();
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
