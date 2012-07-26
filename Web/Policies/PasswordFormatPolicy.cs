namespace Marina.Store.Web.Policies
{
    public class PasswordFormatPolicy
    {
         public const int MIN_PASSWORD_LENGTH = 4; 

         public virtual bool Check(string password)
         {
             return password != null && password.Length >= MIN_PASSWORD_LENGTH;
         }
    }
}