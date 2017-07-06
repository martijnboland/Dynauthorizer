using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Dynauthorizer
{
    public class RuleSet : IRule
    {
        private readonly List<IRule> _rules;

        public IEnumerable<IRule> Rules
        {
            get { return _rules; }
        }

        public RuleSetOperator Operator { get; set; }

        public RuleSet() : this(new List<IRule>(), RuleSetOperator.And)
        {}

        public RuleSet(IEnumerable<IRule> rules, RuleSetOperator ruleSetOperator)
        {
            _rules = new List<IRule>(rules);
            Operator = ruleSetOperator;
        }
        public void Add(IRule rule)
        {
            _rules.Add(rule);
        }

        public bool IsSatisfiedBy(ClaimsPrincipal principal)
        {
            return Operator == RuleSetOperator.Or
                ? Rules.Any(r => r.IsSatisfiedBy(principal))
                : Rules.All(r => r.IsSatisfiedBy(principal));
        }
    }
}
