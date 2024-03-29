﻿using CleanArchitecture.Shared.Core.Result;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace CleanArchitecture.Shared.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();
            
            if (failures.Count != 0)
            {
                Type responseType = typeof(TResponse);
                if (responseType == typeof(Result) || responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    MethodInfo? errorMethod = responseType.GetMethod(nameof(Result.Error));
                    var result = errorMethod?.Invoke(responseType, new[] { failures });
                    return Task.FromResult((TResponse)result!);
                }

                throw new ArgumentException($"Response is not of type {nameof(Result)}.");
            }

            return next();
        }
    }
}
