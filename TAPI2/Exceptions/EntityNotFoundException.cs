using System;
using System.Net;

namespace TAPI2.Exceptions
{
    public class EntityNotFoundException: BusinessException
    {
        public override HttpStatusCode HttpStatusCode { get; protected set; } = HttpStatusCode.NotFound;

        public EntityNotFoundException() 
            : base() 
        { }

        public EntityNotFoundException(string message) 
            : base(message) 
        { }

        public EntityNotFoundException(string message, Exception innerException) 
            : base(message, innerException) 
        { }   
    }
}