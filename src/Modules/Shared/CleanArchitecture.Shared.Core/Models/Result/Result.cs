#nullable enable

namespace CleanArchitecture.Shared.Core.Models.Result
{
    public class Result : IResult
    {
        protected Result(ResultStatus status, string? error = null)
        {
            this.Status = status;
            this.ErrorMessage = error;
        }

        public ResultStatus Status { get; protected set; }

        public string? ErrorMessage { get; protected set; }

        public IList<(string Identifier, string Description)>? ValidationErrors { get; protected set; }

        public object? GetValue() => null;

        public void AddError(string identifier, string description)
        {
            if (this.ValidationErrors is null)
            {
                this.ValidationErrors = new List<(string, string)>();
            }

            this.ValidationErrors.Add((identifier, description));
        }

        public static Result Success() => new(ResultStatus.SuccessNoResult);

        public static Result BadRequest(string errorIdentifier, string errorDescription)
        {
            var result = new Result(ResultStatus.BadRequest);
            result.AddError(errorIdentifier, errorDescription);
            return result;
        }

        public static Result BadRequest(string? error = null) => new(ResultStatus.BadRequest, error);

        public static Result Unauthorized(string? error = null) => new(ResultStatus.Unauthorized, error);

        public static Result Forbidden(string? error = null) => new(ResultStatus.Forbidden, error);

        public static Result NotFound(string? error = null) => new(ResultStatus.NotFound, error);
    }

    public class Result<T> : Result, IResult
        where T : class
    {
        private Result(ResultStatus status, string? error = null)
            : base(status, error)
        {
        }

        private Result(T value)
            : base(ResultStatus.Success)
        {
           this.Value = value;
        }

        public T? Value { get; private set; }

#pragma warning disable CS8603 // Possible null reference return.
        public static implicit operator T(Result<T> result) => result.Value;
#pragma warning restore CS8603 // Possible null reference return.

        public static implicit operator Result<T>(T value) => Success(value);

        public new object? GetValue() => this.Value;

        public static Result<T> Success(T value) => new Result<T>(value);

        public static new Result<T> BadRequest(string errorIdentifier, string errorDescription)
        {
            var result = new Result<T>(ResultStatus.BadRequest);
            result.AddError(errorIdentifier, errorDescription);
            return result;
        }

        public static new Result<T> BadRequest(string? error = null) => new(ResultStatus.BadRequest, error);

        public static new Result<T> Unauthorized(string? error = null) => new(ResultStatus.Unauthorized, error);

        public static new Result<T> Forbidden(string? error = null) => new(ResultStatus.Forbidden, error);

        public static new Result<T> NotFound(string? error = null) => new(ResultStatus.NotFound, error);
    }
}
