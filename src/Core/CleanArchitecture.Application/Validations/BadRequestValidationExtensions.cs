using CleanArchitecture.Domain.Exceptions;

namespace CleanArchitecture.Application.Validations
{
    public static class BadRequestValidationExtensions
    {
        public static void AssertIsTrue(this bool value, string errorMessage)
        {
            if (!value)
            {
                throw new BadRequestException(errorMessage);
            }
        }
    }
}
