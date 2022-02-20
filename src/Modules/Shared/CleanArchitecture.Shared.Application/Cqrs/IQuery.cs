using MediatR;

namespace CleanArchitecture.Shared.Application.Cqrs
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
