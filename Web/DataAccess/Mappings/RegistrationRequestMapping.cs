using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class RegistrationRequestMapping : EntityTypeConfiguration<RegistrationRequest>
    {
        public RegistrationRequestMapping()
        {
            Property(r => r.Email).IsRequired();
            HasRequired(r => r.Cart);
        }
    }
}