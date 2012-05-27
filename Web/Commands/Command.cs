using System.Collections.Generic;

namespace Marina.Store.Web.Commands
{
    public abstract class Command
    {
        public bool HasErrors { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Errors { get; set; }
    }
}