using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class CartItemMapping : EntityTypeConfiguration<CartItem>
    {
        public CartItemMapping()
        {
            Property(a => a.Amount).IsRequired();
            Property(a => a.Price).IsRequired();
            HasRequired(i => i.Product);
        }
    }
}