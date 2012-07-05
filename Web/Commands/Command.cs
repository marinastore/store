using System.Collections.Generic;

namespace Marina.Store.Web.Commands
{
    /// <summary>
    /// Базовый класс для комманд
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Вернуть данные
        /// </summary>
        protected CommandResult<T> Result<T>(T data)
        {
            return new CommandResult<T>(data);
        }

        /// <summary>
        /// Вернуть уведомление об успешном выполнении комманды
        /// </summary>
        protected CommandResult Success()
        {
            return new CommandResult();
        }

        /// <summary>
        /// Вернуть ошибку
        /// </summary>
        protected CommandResult<T> Fail<T>(string key, string value)
        {
            return new CommandResult<T>(new Dictionary<string, string> { { key, value } });
        }

        /// <summary>
        /// Вернуть ошибку
        /// </summary>
        protected CommandResult Fail(string key, string value)
        {
            return new CommandResult(new Dictionary<string, string> {{key, value}});
        }

        /// <summary>
        /// Вернуть ошибки
        /// </summary>
        protected CommandResult<T> Fail<T>(IDictionary<string, string> errors)
        {
            return new CommandResult<T>(errors);
        }

        /// <summary>
        /// Вернуть ошибки
        /// </summary>
        protected CommandResult Fail(IDictionary<string, string> errors)
        {
            return new CommandResult(errors);
        }
    }
}