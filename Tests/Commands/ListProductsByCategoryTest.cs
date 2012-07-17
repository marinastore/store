using System.Linq;
using Marina.Store.Web.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class ListProductsByCategoryTest : CommandTestBase
    {
        /// <summary>
        /// Продукты возвращаются
        /// </summary>
        [TestMethod]
        public void Must_list_products()
        {
            // Arrange

            var category = CreateCategory();
            Repeat(() => CreateProduct(category), 2);
            Db.SaveChanges();

            // Act

            var cmd = new ListProductsByCategoryCommand(Db);
            var result = cmd.Execute(category.Id);

            // Assert

            AssertSuccess(result);
            Assert.IsTrue(result.Value.Any(), "Не возвратились продукты");
            Assert.AreEqual(2, result.Value.Count, "Возвратились не все продукты, либо врозвратились лишние");
            Assert.IsTrue(result.Value.All(p => p.Params.Any()), "Не возвратились параметры продуктов");
        }

        /// <summary>
        /// Возвращаются продукты только для указанной категории
        /// </summary>
        [TestMethod]
        public void Must_list_products_only_by_specified_category()
        {
            // Arrange

            Repeat(() => CreateProduct(), 2);
            var category = CreateCategory();
            Repeat(() => CreateProduct(category), 2);
            Db.SaveChanges();

            // Act

            var cmd = new ListProductsByCategoryCommand(Db);
            var result = cmd.Execute(category.Id);

            // Assert

            AssertSuccess(result);
            Assert.AreEqual(2, result.Value.Count, "Возвратилось неверное кол-во товаров");
            Assert.IsTrue(result.Value.All(p => p.CategoryId == category.Id), "Возвратились товары из другой категории");
        }

        /// <summary>
        /// Для большого кол-ва продуктов в категории
        /// Список продуктов возвращается постранично
        /// </summary>
        [TestMethod]
        public void When_there_are_lots_of_products_Must_paginate()
        {
            // Arrangе

            var category = CreateCategory();
            Repeat(() => CreateProduct(category), 50);
            Db.SaveChanges();

            // Act

            var cmd = new ListProductsByCategoryCommand(Db);
            var result = cmd.Execute(category.Id);

            // Assert

            AssertSuccess(result);
            Assert.AreEqual(50, result.Value.TotalCount, "Вернулось неправильное кол-во товаров в категории");
            Assert.AreEqual(25, result.Value.Count, "Возвратилось неправильное кол-во товаров первой страницы");

            // Act 2

            var result2 = cmd.Execute(category.Id, 30);

            // Assert 2

            AssertSuccess(result2);
            Assert.AreEqual(50, result2.Value.TotalCount, "Вернулось неправильное кол-во товаров в категории");
            Assert.AreEqual(20, result2.Value.Count, "Возвратилось неправильное кол-во товаров второй страницы");
        }
    }
}