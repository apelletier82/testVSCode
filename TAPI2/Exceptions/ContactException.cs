using System.Net;
using System;

namespace TAPI2.Exceptions
{
    public class ContactException : Exception
    {        
        public ExceptionType ExceptionType { get; protected set; }
        public virtual HttpStatusCode HttpStatusCode { get; protected set; } = HttpStatusCode.InternalServerError;

        public ContactException(ExceptionType exceptionType)
            : base() => ExceptionType = exceptionType;        

        public ContactException(ExceptionType exceptionType, string message)
            : base(message)
            => ExceptionType = exceptionType;

        public ContactException(ExceptionType exceptionType, string message, Exception innerException)
            : base(message, innerException)
            => ExceptionType = exceptionType;
    }
}