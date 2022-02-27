#nullable enable

using FluentValidation.Results;

namespace CleanArchitecture.Shared.Core.Result
{
    public interface IResult
    {
        ResultStatus Status { get; }

        public string? ErrorMessage { get; }

        public IList<ValidationFailure>? ValidationErrors { get; }

        object? GetValue();
    }
}
