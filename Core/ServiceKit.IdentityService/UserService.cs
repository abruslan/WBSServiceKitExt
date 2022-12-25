using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ServiceKit.IdentityService
{
    public class UserService
    {
        private readonly IHttpContextAccessor _context;
        public UserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetCurrentUserName()
        {
            return _context.HttpContext?.User?.Identity?.Name;
        }
        public ClaimsPrincipal GetCurrentUser()
        {
            return _context.HttpContext?.User;
        }
    }
}
