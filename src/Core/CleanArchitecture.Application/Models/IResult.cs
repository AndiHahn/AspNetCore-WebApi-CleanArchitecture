using System.Collections.Generic;

#nullable enable

namespace CleanArchitecture.Application.Models
{
    public interface IResult
    {
        ResultStatus Status { get; }

        public string? ErrorMessage { get; }

        public IList<(string Identifier, string Description)>? ValidationErrors { get; }

        object? GetValue();
    }
}
