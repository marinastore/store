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
                db.Categories.SqlQuery("delete from Categories");
                db.Categories.Add( new Category {Id = 1, Name = "Флешки"} );
                db.Categories.Add(new Category { Id = 2, Name = "Левая категория" });
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Продукты возвращаются
        /// </summary>
        [TestMethod]
        public void Must_list_products()
        {
            GenerateProducts(2);

            using(var db = new StoreDbContext())
            {
                var cmd = new ListProductsByCategoryCommand(db);
                var result = cmd.Execute(1);

                Assert.IsNotNull(result);
                Assert.IsFalse(result.HasErrors);
                Assert.IsTrue(result.Model.Any());
                Assert.AreEqual(2, result.Model.Count);
                Assert.IsTrue(result.Model.All( p => p.Params.Count == 2));

                Assert.IsTrue(result.Model.First().Params.Any());
            }
        }

        /// <summary>
        /// Возвращаются продукты только для указанной категории
        /// </summary>
        [TestMethod]
        public void Must_list_products_only_by_specified_category()
        {
            GenerateProducts(2);
            using (var db = new StoreDbContext())
            {
                var cmd = new ListProductsByCategoryCommand(db);
                var result = cmd.Execute(1);
                Assert.AreEqual(2, result.Model.Count);
                Assert.IsTrue(result.Model.All(p => p.CategoryId == 1));
            }
        }

        /// <summary>
        /// Для большого кол-ва продуктов в категории
        /// Список продуктов возвращается постранично
        /// </summary>
        [TestMethod]
        public void When_there_are_lots_of_products_Must_paginate()
        {
            GenerateProducts(50);
            using (var db = new StoreDbContext())
            {
                var cmd = new ListProductsByCategoryCommand(db);
                var result = cmd.Execute(1);
                Assert.AreEqual(50, result.Model.TotalCount);
                Assert.AreEqual(25, result.Model.Count);

                var result2 = cmd.Execute(1, 30);
                Assert.AreEqual(50, result2.Model.TotalCount);
                Assert.AreEqual(20, result2.Model.Count);
            }
        }

        private static void GenerateProducts(int count)
        {
            using (var db = new StoreDbContext())
            {
                db.Products.SqlQuery("delete from Products");
                var categories = db.Categories.ToArray();


                for (var i = 0; i < count; i++)
                {
                    var product = GenerateProduct();
                    product.Category = categories[0];
                    db.Products.Add(product);
                }
                var product2 = GenerateProduct();
                product2.Category = categories[1];
                db.Products.Add(product2);

                db.SaveChanges();
            }
        }

        private static Product GenerateProduct()
        {
            var product = new Product
            {
                Name = "Флеш-память",
                Description = "",
                Vendor = "Transert",
                Price = 900,
                Availability = (int)ProductAvailability.Few,
                Params = new Collection<Param>()
            };
            product.Params.Add(new Param { Name = "Model", Value = "1" });
            product.Params.Add(new Param { Name = "Capacity", Value = "32gb" });
            return product;
        }
    }
}