using System.Collections.Generic;

namespace Marina.Store.Web.Commands
{
    public class CommandResult
    {
        private readonly IDictionary<string, string> _errors;

        public CommandResult(IDictionary<string, string> errors = null)
        {
            _errors = errors ?? new Dictionary<string, string>();
        }

        public bool HasErrors
        {
            get { return Errors != null && Errors.Count > 0; }
        }

        public IDictionary<string, string> Errors
        {
            get { return _errors; }
        }
    }

    public class CommandResult<T> : CommandResult
    {
        public CommandResult(IDictionary<string, string> errors = null) : base(errors)
        {
        }

        public CommandResult(T model, IDictionary<string, string> errors = null) : base(errors)
        {
            Model = model;
        }

        public T Model { get; set; }
    }
}