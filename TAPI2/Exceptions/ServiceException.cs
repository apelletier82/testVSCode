using System;

namespace TAPI2.Exceptions
{
    public class ServiceException: ContactException 
    {             
        public ServiceException() : base(ExceptionType.Service) { }

        public ServiceException(string message) : base(ExceptionType.Service, message) { }

        public ServiceException(string message, Exception innerException) : base(ExceptionType.Service, message, innerException) { }
    }
}