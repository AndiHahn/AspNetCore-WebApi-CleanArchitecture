#nullable enable

namespace CleanArchitecture.Shared.Core.Models.Result
{
    public interface IResult
    {
        ResultStatus Status { get; }

        public string? ErrorMessage { get; }

        public IList<(string Identifier, string Description)>? ValidationErrors { get; }

        object? GetValue();
    }
}
