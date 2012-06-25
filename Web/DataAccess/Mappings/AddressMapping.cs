using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class AddressMapping : EntityTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            Property(a => a.Metro).IsOptional().HasMaxLength(30);
            Property(a => a.City).IsRequired().HasMaxLength(30);
            Property(a => a.StreetAddress).IsRequired().HasMaxLength(200);
        }
    }
}