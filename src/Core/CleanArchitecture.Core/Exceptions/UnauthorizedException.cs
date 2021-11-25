using System;
using System.Runtime.Serialization;

namespace CleanArchitecture.Core.Exceptions
{
    /// <summary>
    /// Exception, which is used to indicate a HTTP status code 401
    /// </summary>
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
