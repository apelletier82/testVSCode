using Microsoft.AspNetCore.Http;
using TAPI2.Services.Abstract;

namespace TAPI2.Services
{
    public class HttpContextUserIdentity : IUserIdentity
    {
        IHttpContextAccessor _context;

        public HttpContextUserIdentity(IHttpContextAccessor contextAccessor) 
            =>_context = contextAccessor;

        public string Username 
            => _context?.HttpContext?.User?.Identity?.Name;
    }
}