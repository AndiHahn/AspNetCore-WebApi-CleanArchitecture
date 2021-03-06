﻿using System;
using System.Runtime.Serialization;

namespace CleanArchitecture.Domain.Exceptions
{
    /// <summary>
    /// Exception, which is used to indicate a HTTP status code 403
    /// </summary>
    [Serializable]
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ForbiddenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
