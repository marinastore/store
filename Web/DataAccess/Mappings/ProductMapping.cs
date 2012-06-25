using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class ProductMapping : EntityTypeConfiguration<Product>
    {
        public ProductMapping()
        {
            Property(p => p.Name).IsRequired().HasMaxLength(100);
            Property(p => p.Vendor).IsRequired().HasMaxLength(50);
            Property(p => p.Price).IsRequired();
            Property(p => p.Description).IsOptional().HasMaxLength(500);
            Property(p => p.Picture).IsOptional().HasMaxLength(100);
            Property(p => p.Availability).IsRequired();
            HasRequired(p => p.Category);
        }
    }
}