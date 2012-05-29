using System.Collections.Generic;

namespace Marina.Store.Web.Commands
{
    public class CommandResult<T>
    {
        private readonly IDictionary<string, string> _errors;

        public CommandResult()
        {
            _errors = new Dictionary<string, string>();
        }

        public bool HasErrors
        {
            get { return Errors != null && Errors.Count > 0; }
        }

        public IDictionary<string, string> Errors
        {
            get { return _errors; }
        }

        public T Data { get; set; }
    }
}