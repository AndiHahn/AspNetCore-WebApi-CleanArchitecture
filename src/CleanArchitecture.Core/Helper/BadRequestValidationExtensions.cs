using CleanArchitecture.Core.Exceptions;

namespace CleanArchitecture.Core.Helper
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
