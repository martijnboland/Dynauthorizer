using Xunit;

namespace Dynauthorizer.Tests
{
    public class RuleSetTests
    {
        [Fact]
        public void Both_claims_must_be_satisified_in_RuleSet_with_2_rules_and_operator_And()
        {
            var rule1 = new ClaimRule {ClaimType = "testclaim1", ClaimValue = "testvalue1"};
            var rule2 = new ClaimRule {ClaimType = "testclaim2", ClaimValue = "testvalue2"};
            var ruleSet = new RuleSet(new [] { rule1, rule2 }, RuleSetOperator.And);
            var principalWithBothClaims = new ClaimsPrincipalBuilder().WithClaim("testclaim1", "testvalue1").WithClaim("testclaim2", "testvalue2").Build();
            var principalWithOnlyOneClaim = new ClaimsPrincipalBuilder().WithClaim("testclaim1", "textvalue1").Build();

            Assert.True(ruleSet.IsSatisfiedBy(principalWithBothClaims));
            Assert.False(ruleSet.IsSatisfiedBy(principalWithOnlyOneClaim));
        }

        [Fact]
        public void Only_one_claim_must_be_satisfied_in_RuleSet_with_2_rules_and_operator_Or()
        {
            var rule1 = new ClaimRule { ClaimType = "testclaim1", ClaimValue = "testvalue1" };
            var rule2 = new ClaimRule { ClaimType = "testclaim2", ClaimValue = "testvalue2" };
            var ruleSet = new RuleSet(new[] { rule1, rule2 }, RuleSetOperator.Or);
            var principalWithOnlyOneClaim = new ClaimsPrincipalBuilder().WithClaim("testclaim1", "testvalue1").Build();

            Assert.True(ruleSet.IsSatisfiedBy(principalWithOnlyOneClaim));
        }

        [Fact]
        public void Composite_RuleSet_with_one_Rule_and_a_nested_RuleSet_and_operator_or_can_be_satisfied_with_a_principal_that_matches_the_Rule_but_also_with_a_principal_that_matches_the_nested_ruleset()
        {
            var adminRoleRule = new ClaimRule { ClaimType = "role", ClaimValue = "admin" };
            var powerUserRoleRule = new ClaimRule { ClaimType = "role", ClaimValue = "power users" };
            var salesDepartmentRule = new ClaimRule {ClaimType = "department", ClaimValue = "sales"};
            var salesPowerUsersRuleSet = new RuleSet(new [] { powerUserRoleRule, salesDepartmentRule }, RuleSetOperator.And);
            var compositeRuleSet = new RuleSet(new IRule[] { adminRoleRule, salesPowerUsersRuleSet}, RuleSetOperator.Or);
            var adminPrincipal = new ClaimsPrincipalBuilder().WithClaim("role", "admin").Build();
            var salesPowerUserPrincipal = new ClaimsPrincipalBuilder().WithClaim("role", "power users").WithClaim("department", "sales").Build();
            var regularSalesUser = new ClaimsPrincipalBuilder().WithClaim("role", "users").WithClaim("department", "sales").Build();

            Assert.True(compositeRuleSet.IsSatisfiedBy(adminPrincipal));
            Assert.True(compositeRuleSet.IsSatisfiedBy(salesPowerUserPrincipal));
            Assert.False(compositeRuleSet.IsSatisfiedBy(regularSalesUser));
        }
    }
}
