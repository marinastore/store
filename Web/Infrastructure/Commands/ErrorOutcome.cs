namespace Marina.Store.Web.Infrastructure.Commands
{
    public class ErrorOutcome : Outcome
    {
        public ErrorOutcome(string name, string description)
            : base(name, description)
        {
        }
    }
}