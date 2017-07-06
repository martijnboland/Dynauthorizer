using System;
using System.Security.Claims;

namespace Dynauthorizer
{
    public class RulePolicy
    {
        public string Name { get; set; }

        public IRule RootRule { get; set; }

        public bool IsSatisfiedBy(ClaimsPrincipal principal)
        {
            if (RootRule == null)
            {
                throw new InvalidOperationException("Cannot validate RulePolicy when RootRule is null");
            }
            return RootRule.IsSatisfiedBy(principal);
        }
    }
}
