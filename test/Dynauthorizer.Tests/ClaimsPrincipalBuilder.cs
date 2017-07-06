using System.Collections.Generic;
using System.Security.Claims;

namespace Dynauthorizer.Tests
{
    public class ClaimsPrincipalBuilder
    {
        private readonly List<Claim> _claims;

        public ClaimsPrincipalBuilder()
        {
            _claims = new List<Claim>();
        }

        public ClaimsPrincipalBuilder WithClaim(string claimType, string claimValue)
        {
            _claims.Add(new Claim(claimType, claimValue));
            return this;
        }

        public ClaimsPrincipal Build()
        {
            var identity = new ClaimsIdentity(_claims, "Test", "name", "role");
            identity.AddClaim(new Claim("name", "testprincipal"));

            return new ClaimsPrincipal(new ClaimsIdentity(_claims));
        }
    }
}
