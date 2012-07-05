namespace Marina.Store.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public Address PrimaryAddress { get; set; }

        public Address SecondaryAddress { get; set; }
    }
}