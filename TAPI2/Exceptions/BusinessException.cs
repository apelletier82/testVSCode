using System;
using System.Net;

namespace TAPI2.Exceptions
{
    public class BusinessException : ContactException
    {
        public override HttpStatusCode HttpStatusCode { get; protected set; } = HttpStatusCode.BadRequest;

        public BusinessException() 
            : base(ExceptionType.Business)
        { }

        public BusinessException(string message) 
            : base(ExceptionType.Business, message)
        { }

        public BusinessException(string message, Exception innerException) 
            : base(ExceptionType.Business, message, innerException)
        { }
    }
}