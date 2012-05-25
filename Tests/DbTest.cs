using System.Linq;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests
{
    [TestClass]
    public class DbTest
    {
        [TestInitialize]
        public void EmptyProducts()
        {
            using (var db = new StoreDbContext())
            {
                db.Products.SqlQuery("delete from Products");
            }
        }

        [TestMethod]
        public void Must_save_product()
        {
            var product = new Product { Availability = ProductAvailability.Plenty };

            using (var db = new StoreDbContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }

        } 
 
        [TestMethod]
        public void Must_get_product()
        {
            using (var db = new StoreDbContext())
            {
                db.Products.Add(new Product {Availability = ProductAvailability.Plenty});
                db.SaveChanges();
            }

            using (var db = new StoreDbContext())
            {
                var product = db.Products.FirstOrDefault();
                Assert.IsNotNull(product);
                Assert.AreNotEqual(0, product.Id);
            }
        }
    }
}