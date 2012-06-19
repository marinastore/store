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

        public DbSet<User> Users { get; set; }

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
            modelBuilder.Entity<User>().HasOptional(u => u.PrimaryAddress);
            modelBuilder.Entity<User>().HasOptional(u => u.SecondaryAddress);
            modelBuilder.Entity<User>().Property(u => u.Phone).IsOptional();
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
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

        private void SetupAddress(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<Address>().Property(a => a.Metro).IsOptional().HasMaxLength(30);
            modelBuilder.Entity<Address>().Property(a => a.City).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Address>().Property(a => a.StreetAddress).IsRequired().HasMaxLength(200);
        }


        private static void SetupCategory(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().Property(a => a.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Category>().Property(a => a.Description).IsOptional().HasMaxLength(500);
            modelBuilder.Entity<Category>().Property(a => a.Picture).IsOptional().HasMaxLength(100);
        }

        private static void SetupShoppingCart(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>().HasMany(c => c.Items);
            modelBuilder.Entity<ShoppingCart>().HasOptional(c => c.User);
            modelBuilder.Entity<ShoppingCart>().Ignore(c => c.Total);
        }

        private static void SetupCartItem(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>().HasRequired(i => i.Product);
            modelBuilder.Entity<CartItem>().Property(a => a.CreateDate).IsRequired();
            modelBuilder.Entity<CartItem>().Property(a => a.Amount).IsRequired();
        }

        private static void SetupParam(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Param>().HasKey(p => new { p.ProductId, p.Name });
            modelBuilder.Entity<Param>().Property(p => p.ProductId).IsRequired();
            modelBuilder.Entity<Param>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Param>().Property(p => p.Value).IsRequired().HasMaxLength(50);
        }

        private static void SetupProduct(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Vendor).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Description).IsOptional().HasMaxLength(500);
            modelBuilder.Entity<Product>().Property(p => p.Picture).IsOptional().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Availability).IsRequired();
            modelBuilder.Entity<Product>().HasRequired(p => p.Category);
        }
    }
}