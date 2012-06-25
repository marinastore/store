using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            Property(u => u.Phone).IsOptional();
            Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            Property(u => u.LastName).IsRequired().HasMaxLength(50);
            HasOptional(u => u.PrimaryAddress);
            HasOptional(u => u.SecondaryAddress);
        }
    }
}