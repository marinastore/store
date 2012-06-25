using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class ShoppingCartMapping : EntityTypeConfiguration<ShoppingCart>
    {
        public ShoppingCartMapping()
        {
            HasMany(c => c.Items);
            HasOptional(c => c.User);
            Ignore(c => c.Total);
        }
    }
}