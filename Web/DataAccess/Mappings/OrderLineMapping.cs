using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class OrderLineMapping : EntityTypeConfiguration<OrderLine>
    {
        public OrderLineMapping()
        {
            Property(l => l.ProductName).IsRequired().HasMaxLength(100);
            Property(l => l.Amount).IsRequired();
            Property(l => l.Price).IsRequired();
            HasRequired(l => l.Product);
        }
    }
}