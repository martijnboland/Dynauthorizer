using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Dynauthorizer.Tests
{
    public class RulePolicySerializationTests
    {
        public RulePolicySerializationTests()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        [Fact]
        public void A_RulePolicy_can_be_properly_serialized_to_json()
        {
            var adminRoleRule = new ClaimRule { ClaimType = "role", ClaimValue = "admin" };
            var powerUserRoleRule = new ClaimRule { ClaimType = "role", ClaimValue = "power user" };
            var salesDepartmentRule = new ClaimRule { ClaimType = "department", ClaimValue = "sales" };
            var nameDynauthorizerRule = new ClaimRule { ClaimType = "name", ClaimValue = "Dynauthorizer" };

            var salesPowerUserRuleSet = new RuleSet(new[] { powerUserRoleRule, salesDepartmentRule }, RuleSetOperator.And);
            var rootRuleSet = new RuleSet(new IRule[] { adminRoleRule, nameDynauthorizerRule, salesPowerUserRuleSet }, RuleSetOperator.Or );

            var policy = new RulePolicy { Name = "MyPolicy", RootRule = rootRuleSet };

            var jsonString = JsonConvert.SerializeObject(policy, Formatting.Indented);

            var expectedString = @"{
  ""name"": ""MyPolicy"",
  ""rootRule"": {
    ""rules"": [
      {
        ""claimType"": ""role"",
        ""claimValue"": ""admin""
      },
      {
        ""claimType"": ""name"",
        ""claimValue"": ""Dynauthorizer""
      },
      {
        ""rules"": [
          {
            ""claimType"": ""role"",
            ""claimValue"": ""power user""
          },
          {
            ""claimType"": ""department"",
            ""claimValue"": ""sales""
          }
        ],
        ""operator"": ""And""
      }
    ],
    ""operator"": ""Or""
  }
}";
            Assert.Equal(expectedString, jsonString);
        }

        [Fact]
        public void A_policy_can_be_properly_deserialized_from_json()
        {
            var jsonString = File.ReadAllText("Policies/AdminDynauthorizerSalesPowerUserPolicy.json");

            var policy = JsonConvert.DeserializeObject<RulePolicy>(jsonString);

            Assert.NotNull(policy);
            Assert.IsType<RuleSet>(policy.RootRule);

        }
    }
}
