using System.Data.Entity;
using Marina.Store.Web.DataAccess.Mappings;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess
{
    public class StoreDbContext : DbContext
    {
        public IDbSet<Product> Products { get; set; }

        public IDbSet<ShoppingCart> ShoppingCarts { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations
                .Add(new ProductMapping())
                .Add(new ParamMapping())
                .Add(new CategoryMapping())
                .Add(new AddressMapping())
                .Add(new UserMapping())
                .Add(new ShoppingCartMapping())
                .Add(new CartItemMapping())
                .Add(new OrderMapping())
                .Add(new OrderLineMapping())
                ;
        }
    }
}