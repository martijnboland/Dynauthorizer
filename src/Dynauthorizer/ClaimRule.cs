using System.Security.Claims;

namespace Dynauthorizer
{
    public class ClaimRule : IRule
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public bool IsSatisfiedBy(ClaimsPrincipal principal)
        {
            // Only check claim type when claim value is null
            return ClaimValue == null
                ? principal.HasClaim(c => c.Type == ClaimType)
                : principal.HasClaim(ClaimType, ClaimValue);
        }
    }
}
