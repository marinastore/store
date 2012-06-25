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
                ;

            // TODO: создать маппинги по образу и подобию остальных сущностей
            SetupOrder(modelBuilder);
            SetupOrderLine(modelBuilder);
        }

        private static void SetupOrderLine(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLine>().Property(l => l.ProductName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<OrderLine>().Property(l => l.Amount).IsRequired();
            modelBuilder.Entity<OrderLine>().Property(l => l.Price).IsRequired();
            modelBuilder.Entity<OrderLine>().HasRequired(l => l.Product);
        }

        private static void SetupOrder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().HasMany(o => o.Lines);
            modelBuilder.Entity<Order>().HasRequired(o => o.Address);
            modelBuilder.Entity<Order>().Property(o => o.Phone).IsRequired();
            modelBuilder.Entity<Order>().Property(o => o.Comment).IsOptional().HasMaxLength(200);
            modelBuilder.Entity<Order>().Property(o => o.CreateDate).IsRequired();
            modelBuilder.Entity<Order>().Ignore(o => o.Total);
            modelBuilder.Entity<Order>().HasOptional(o => o.User);
           
        }
    }
}