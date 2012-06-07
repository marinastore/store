using System.Collections.ObjectModel;
using Marina.Store.Web.Commands;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class GetProductTest
    {

        [TestInitialize]
        public void Init()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Флеш-память",
                Description = "",
                Vendor = "Transert",
                Price = 900,
                Availability = (int)ProductAvailability.Few,
                Category = new Category
                {
                    Name = "Флешки",
                    Description = "Флешки"
                },
                Params = new []
                {
                    new Param
                    {
                        Name = "Размер памяти",
                        Value = "5гб"
                    },
                    new Param
                    {
                        Name = "Цвет",
                        Value = "Красный"
                    }
                }
            };

            using (var db = new StoreDbContext())
            {
                db.Products.SqlQuery("delete from Products");
                db.Products.SqlQuery("delete from Categories");
                db.Products.SqlQuery("delete from Params");
                db.Products.Add(product);
                db.SaveChanges();
            }

        }

        /// <summary>
        /// Возвращается товар
        /// </summary>
        [TestMethod]
        public void Must_return_product()
        {
            using (var db = new StoreDbContext())
            {
                var cmd = new GetProductCommand(db);
                var result = cmd.Execute(1);
                Assert.IsNotNull(result);
                Assert.IsFalse(result.HasErrors);
                Assert.AreEqual(1, result.Model.Id);
                Assert.IsNotNull(result.Model.Category);
                Assert.AreEqual("Флеш-память", result.Model.Name);
                Assert.IsNotNull(result.Model.Params);
                Assert.AreEqual(2, result.Model.Params.Count);
            }  
           

        }
    }
}