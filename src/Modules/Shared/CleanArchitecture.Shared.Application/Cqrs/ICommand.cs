using MediatR;

namespace CleanArchitecture.Shared.Application.Cqrs
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
