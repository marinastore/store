namespace Marina.Store.Web.Infrastructure.Commands
{
    public abstract class Outcome
    {
        protected Outcome(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
    }
}