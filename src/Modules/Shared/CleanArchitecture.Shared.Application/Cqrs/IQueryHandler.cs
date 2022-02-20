using MediatR;

namespace CleanArchitecture.Shared.Application.Cqrs
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}
