using System;
using System.Runtime.Serialization;

namespace CleanArchitecture.Domain.Exceptions
{
    /// <summary>
    /// Exception, which is used to indicate a HTTP status code 400
    /// </summary>
    [Serializable]
    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConflictException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}