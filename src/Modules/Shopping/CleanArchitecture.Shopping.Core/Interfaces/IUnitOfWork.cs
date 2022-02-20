using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IBankAccountRepository BankAccountRepository { get; }
        public IBillRepository BillRepository { get; }
        public IUserRepository UserRepository { get; }

        public Task CommitAsync(CancellationToken cancellationToken);
    }
}
