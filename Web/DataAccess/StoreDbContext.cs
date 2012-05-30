using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            SetupProduct(modelBuilder);
            SetupParam(modelBuilder);
            SetupCartItem(modelBuilder);
            SetupShoppingCart(modelBuilder);
            SetupCategory(modelBuilder);
            SetupAddress(modelBuilder);
            SetupOrder(modelBuilder);
            SetupOrderLine(modelBuilder);
            SetupUser(modelBuilder);
        }

        private static void SetupUser(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasOptional(u => u.PrimaryAddress);
            modelBuilder.Entity<User>().HasOptional(u => u.SecondaryAddress);
            modelBuilder.Entity<User>().Property(u => u.Phone).IsRequired();
        }

        private static void SetupOrderLine(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<OrderLine>().HasKey(l => l.Id);
        }

        private static void SetupOrder(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Order>().HasMany(o => o.Lines);
            modelBuilder.Entity<Order>().HasRequired(o => o.Address);
        }

        private void SetupAddress(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
        }


        private static void SetupCategory(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
        }

        private static void SetupShoppingCart(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<ShoppingCart>().HasKey(c => c.Id);
            modelBuilder.Entity<ShoppingCart>().HasMany(c => c.Items);
            modelBuilder.Entity<ShoppingCart>().HasOptional(c => c.User);
            
        }

        private static void SetupCartItem(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<CartItem>().HasKey(i => i.Id);
            modelBuilder.Entity<CartItem>().HasRequired(i => i.Product);
        }

        private static void SetupParam(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<Param>().HasKey(p => new { p.ProductId, p.Name });
        }

        private static void SetupProduct(DbModelBuilder modelBuilder)
        {
            // TODO: описать все поля (обязательность, длины строк и т.п.)
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}