using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using System.Security.Claims;

namespace CFTenantPortal.Services
{
    public class RequestInfoService : IRequestInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<UserRoles>? UserRoles
        {
            get
            {
                if (IsLoggedIn)
                {
                    var context = _httpContextAccessor.HttpContext;
                    if (context?.User.Identity is not ClaimsIdentity claimsIdentity) return new();

                    var roles = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Role)
                                .Select(claim => (UserRoles)Enum.Parse(typeof(UserRoles), claim.Value)).ToList();
                    return roles;
                }
                return null;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return _httpContextAccessor.HttpContext != null &&
                    _httpContextAccessor.HttpContext.User.Identity != null &&
                    _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            }
        }

        public string? UserId
        {
            get
            {
                if (IsLoggedIn)
                {
                    var context = _httpContextAccessor.HttpContext;
                    if (context?.User.Identity is not ClaimsIdentity claimsIdentity) return null;

                    return claimsIdentity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                }
                return null;
            }
        }
    }
}
