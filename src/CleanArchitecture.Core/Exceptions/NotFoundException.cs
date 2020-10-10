using System;
using System.Runtime.Serialization;

namespace CleanArchitecture.Core.Exceptions
{
    /// <summary>
    /// Exception, which is used to indicate a HTTP status code 404
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}