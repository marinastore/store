using System.Linq;
using Marina.Store.Web.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class AddToShoppingCartTest : CommandTestBase
    {
        /// <summary>
        /// Товар добавляется в корзину
        /// </summary>
        [TestMethod]
        public void Must_add_to_cart()
        {
            // Arrange

            var user = CreateUser();
            CreateEmptyCart(user);
            CreateProduct();
            var product = CreateProduct(); // тестируемый продукт
            CreateProduct();
            Db.SaveChanges();

            // Act

            var getCartCmd = new GetShoppingCartCommand(Db, user);
            var cmd = new AddToShoppingCartCommand(Db, getCartCmd);
            var result = cmd.Execute(product.Id);

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            var cartResult = getCartCmd.Execute();
            Assert.AreEqual(1, cartResult.Model.Items.Count, "Товар не добавился в корзину"); 
            Assert.AreEqual(product.Id, cartResult.Model.Items.First().ProductId, "Добавился не тот продукт");
        }

        /// <summary>
        /// Если такой товар уже в корзине
        /// Увеличивается его кол-во (а не добавляется новый товар)
        /// </summary>
        [TestMethod]
        public void When_given_product_found_in_the_cart_Must_increment_amount()
        {
            // Arrange

            var user = CreateUser();
            CreateEmptyCart(user);
            var product = CreateProduct();
            Db.SaveChanges();

            // Act

            var getCartCmd = new GetShoppingCartCommand(Db, user);
            var cmd = new AddToShoppingCartCommand(Db, getCartCmd);
            cmd.Execute(product.Id);
            var result = cmd.Execute(product.Id, 2);

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            var cartResult = getCartCmd.Execute();
            Assert.AreEqual(1, cartResult.Model.Items.Count, "Добавился новый товар");
            Assert.AreEqual(3, cartResult.Model.Items.First().Amount, "Количество товара не увеличилось");
        }
    }
}