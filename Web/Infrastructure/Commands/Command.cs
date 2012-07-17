using System;
using System.Linq.Expressions;

namespace Marina.Store.Web.Infrastructure.Commands
{
    /// <summary>
    /// Базовый класс для комманд
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Все команды имают состояние успешного завершения
        /// </summary>
        public State Success;

        /// <summary>
        /// Вернуть результат, соответствующий завершению в состоянии с ошибкой
        /// </summary>
        protected Outcome Fail(Expression<Func<State>> stateProperty, string description)
        {
            var name = ExtractProperty(stateProperty);
            if (name == ExtractProperty(()=>Success))
            {
                throw new InvalidOperationException("Метод Fail не может вернуть состояние Success");
            }
            return new ErrorOutcome(name, description);
        }

        /// <summary>
        /// Вернуть результат, соответствующий успешному завершения комманды
        /// </summary>
        protected Outcome Ok(string description = null)
        {
            return new SuccessOutcome(description);
        }

        /// <summary>
        /// Вернуть значение
        /// </summary>
        protected Result<T> Value<T>(T value, string description = null)
        {
            return new Result<T>(new SuccessOutcome(description), value);
        }

        private static string ExtractProperty<T>(Expression<Func<T>> property)
        {
            var expression = (MemberExpression)property.Body;
            return expression.Member.Name;
        }
    }
}