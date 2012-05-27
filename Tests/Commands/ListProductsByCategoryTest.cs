using System.Linq;
using Marina.Store.Web.Commands;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class ListProductsByCategoryTest
    {
        [TestMethod]
        public void Must_list_products()
        {
            GenerateProducts(2);

            using(var db = new StoreDbContext())
            {
                var cmd = new ListProductsByCategoryCommand(db);
                var products = cmd.ListProducts(0);

                Assert.IsNotNull(products);
                Assert.IsTrue(products.Any());
                Assert.Equals(2, products.Length);
            }
        }

        private static void GenerateProducts(int count)
        {
            using (var db = new StoreDbContext())
            {
                db.Products.SqlQuery("delete from products");
                for (var i = 0; i < count; i++)
                {
                    db.Products.Add(new Product());
                }
                db.SaveChanges();
            }
        }
    }
}