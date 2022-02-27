using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;

namespace CleanArchitecture.Shopping.Application.User.Queries
{
    public class GetCurrentUserQuery : IQuery<Result<UserDto>>
    {
        public GetCurrentUserQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetUserQueryHandler : IQueryHandler<GetCurrentUserQuery, Result<UserDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            return this.userRepository
                .GetByIdAsync(request.CurrentUserId, cancellationToken)
                .ContinueWith(u => Result<UserDto>.Success(this.mapper.Map<UserDto>(u.Result)), cancellationToken);
        }
    }
}
