using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        private readonly IShoppingDbContext dbContext;
        private readonly IMapper mapper;

        public GetUserQueryHandler(
            IShoppingDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await this.dbContext.User.FindByIdAsync(request.CurrentUserId, cancellationToken);
            return Result<UserDto>.Success(this.mapper.Map<UserDto>(user));
        }
    }
}
