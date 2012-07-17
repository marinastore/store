using System;
using System.Linq.Expressions;

namespace Marina.Store.Web.Infrastructure.Commands
{
    public class SuccessOutcome : Outcome
    {
        public SuccessOutcome() : base(GetName(), "OK") { }

        public SuccessOutcome(string description) : base(GetName(), description ?? "OK") { }

        private static string GetName()
        {
            Expression<Func<Command, State>> expression = c => c.Success;
            return ((MemberExpression)expression.Body).Member.Name;
        }
    }
}