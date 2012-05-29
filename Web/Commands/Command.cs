using System.Collections.Generic;

namespace Marina.Store.Web.Commands
{
    public abstract class Command
    {
        protected CommandResult<T> Result<T>(T data)
        {
            return new CommandResult<T> { Model = data };
        }

        protected CommandResult<T> Error<T>(IDictionary<string, string> errors, T data = default(T))
        {
            var result =  new CommandResult<T> { Model = data };
            
            foreach (var error in errors)
            {
                result.Errors.Add(error);
            }

            return result;
        }
    }
}