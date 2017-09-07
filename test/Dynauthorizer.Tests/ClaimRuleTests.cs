using Xunit;

namespace Dynauthorizer.Tests
{
    public class ClaimRuleTests
    {
        [Fact]
        public void Rule_with_matching_principal_returns_true()
        {
            var rule = new ClaimRule { ClaimType = "testclaim", ClaimValue = "testvalue" };
            var principal = new ClaimsPrincipalBuilder().WithClaim("testclaim", "testvalue").Build();

            var isOk = rule.IsSatisfiedBy(principal);

            Assert.True(isOk);
        }

        [Fact]
        public void Rule_without_matching_principal_returns_false()
        {
            var rule = new ClaimRule { ClaimType = "testclaim", ClaimValue = "testvalue" };
            var principal = new ClaimsPrincipalBuilder().Build(); // empty, no claims

            var isOk = rule.IsSatisfiedBy(principal);

            Assert.False(isOk);
        }

        [Fact]
        public void Rule_without_ClaimValue_only_checks_ClaimType()
        {
            var rule = new ClaimRule {ClaimType = "testclaim", ClaimValue = null};
            var principal = new ClaimsPrincipalBuilder().WithClaim("testclaim", "whatever").Build();

            var isOk = rule.IsSatisfiedBy(principal);

            Assert.True(isOk);
        }
    }
}
