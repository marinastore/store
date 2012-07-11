using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            Property(u => u.Email).IsRequired();
            Property(u => u.PasswordHash).IsRequired();
            Property(u => u.Phone).IsOptional();
            Property(u => u.FirstName).IsOptional().HasMaxLength(50);
            Property(u => u.LastName).IsOptional().HasMaxLength(50);
            HasOptional(u => u.PrimaryAddress);
            HasOptional(u => u.SecondaryAddress);
        }
    }
}