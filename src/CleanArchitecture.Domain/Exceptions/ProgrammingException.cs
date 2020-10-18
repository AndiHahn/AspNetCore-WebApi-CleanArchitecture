using System;
using System.Runtime.Serialization;

namespace CleanArchitecture.Domain.Exceptions
{
    /// <summary>
    /// Exception, which is used to indicate a HTTP status code 400
    /// </summary>
    [Serializable]
    public class ProgrammingException : Exception
    {
        public ProgrammingException()
        {
        }

        public ProgrammingException(string message) : base(message)
        {
        }

        public ProgrammingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProgrammingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}
