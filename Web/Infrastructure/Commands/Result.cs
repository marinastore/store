using System;
using System.Linq.Expressions;

namespace Marina.Store.Web.Infrastructure.Commands
{
    public class Result
    {
        internal Result(Outcome outcome)
        {
            if (outcome == null)
            {
                throw new ArgumentNullException("outcome");
            }
            Outcome = outcome;
        }

        public Outcome Outcome { get; protected set; }

        public bool HasErrors
        {
            get
            {
                return !(Outcome is SuccessOutcome);
            }
        }

        public bool Is<T>(Expression<Func<T, State>> what) where T : Command
        {
            var name = ExtractProperty(what);
            return Outcome.Name == name;
        }

        public static implicit operator Result(Outcome outcome)
        {
            return new Result(outcome);
        }

        protected static string ExtractProperty<T>(Expression<Func<T, State>> property)
        {
            var expression = (MemberExpression)property.Body;
            return expression.Member.Name;
        }
    }

    public class Result<T> : Result
    {
        internal Result(Outcome outcome, T data = default(T))
            : base(outcome)
        {
            Value = data;
        }

        public T Value { get; protected set; }

        public static implicit operator Result<T>(Outcome outcome)
        {
            return new Result<T>(outcome);
        }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(new SuccessOutcome(), value);
        }

        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }
    }
}