using System.Security.Claims;

namespace Dynauthorizer
{
    public interface IRule
    {
        bool IsSatisfiedBy(ClaimsPrincipal principal);
    }
}
