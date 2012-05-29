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
        }

        private static void SetupCategory(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
        }

        private static void SetupShoppingCart(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>().HasKey(c => c.Id);
            modelBuilder.Entity<ShoppingCart>().HasMany(c => c.Items);
        }

        private static void SetupCartItem(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>().HasKey(i => i.Id);
            modelBuilder.Entity<CartItem>().HasRequired(i => i.Product);
        }

        private static void SetupParam(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Param>().HasKey(p => new { p.ProductId, p.Name });
        }

        private static void SetupProduct(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}