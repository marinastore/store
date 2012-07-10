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

            AssertCommandSuccess(result);
            Assert.IsTrue(result.Model.Any(), "Не возвратились продукты");
            Assert.AreEqual(2, result.Model.Count, "Возвратились не все продукты, либо врозвратились лишние");
            Assert.IsTrue(result.Model.All(p => p.Params.Any()), "Не возвратились параметры продуктов");
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

            AssertCommandSuccess(result);
            Assert.AreEqual(2, result.Model.Count, "Возвратилось неверное кол-во товаров");
            Assert.IsTrue(result.Model.All(p => p.CategoryId == category.Id), "Возвратились товары из другой категории");
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

            AssertCommandSuccess(result);
            Assert.AreEqual(50, result.Model.TotalCount, "Вернулось неправильное кол-во товаров в категории");
            Assert.AreEqual(25, result.Model.Count, "Возвратилось неправильное кол-во товаров первой страницы");

            // Act 2

            var result2 = cmd.Execute(category.Id, 30);

            // Assert 2

            AssertCommandSuccess(result2);
            Assert.AreEqual(50, result2.Model.TotalCount, "Вернулось неправильное кол-во товаров в категории");
            Assert.AreEqual(20, result2.Model.Count, "Возвратилось неправильное кол-во товаров второй страницы");
        }
    }
}