using System.Collections.ObjectModel;
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
        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            using(var db = new StoreDbContext())
            {
                db.Categories.Add( new Category {Name = "Флешки"} );
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void Must_list_products()
        {
            GenerateProducts(2);

            using(var db = new StoreDbContext())
            {
                var cmd = new ListProductsByCategoryCommand(db);
                var result = cmd.Execute("Флешки");

                Assert.IsNotNull(result);
                Assert.IsFalse(result.HasErrors);
                Assert.IsTrue(result.Model.Any());
                Assert.AreEqual(2, result.Model.Length);

                Assert.IsTrue(result.Model.First().Params.Any());
            }
        }

        private static void GenerateProducts(int count)
        {
            using (var db = new StoreDbContext())
            {
                db.Products.SqlQuery("delete from products");
                var category = db.Categories.First();

                for (var i = 0; i < count; i++)
                {
                    var product = new Product();
                    product.Params = new Collection<Param>();
                    product.Category = category;
                    product.Params.Add(new Param {Name = "Model", Value = "1"});
                    product.Params.Add(new Param {Name = "Capacity", Value = "32gb"});
                    db.Products.Add(product);
                }
                db.SaveChanges();
            }
        }
    }
}