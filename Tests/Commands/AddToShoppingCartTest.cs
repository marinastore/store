using System.Linq;
using Marina.Store.Web.Commands;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

            CreateProduct();
            var product = CreateProduct(); // тестируемый продукт
            CreateProduct();
            var cart = CreateCart();
            Db.SaveChanges();

            // Act

            var cmd = new AddToShoppingCartCommand(MoqGetShoppingCart(cart).Object);
            var result = cmd.Execute(product.Id);

            // Assert

            AssertCommandSuccess(result);
            Assert.AreEqual(1, cart.Items.Count, "Товар не добавился в корзину"); 
            Assert.AreEqual(product.Id, cart.Items.First().ProductId, "Добавился не тот продукт");
        }

        /// <summary>
        /// Если такой товар уже в корзине
        /// Увеличивается его кол-во (а не добавляется новый товар)
        /// </summary>
        [TestMethod]
        public void When_given_product_found_in_the_cart_Must_increment_amount()
        {
            // Arrange

            var product = CreateProduct();
            var cart = CreateCart();
            Db.SaveChanges();

            // Act

            var cmd = new AddToShoppingCartCommand(MoqGetShoppingCart(cart).Object);
            cmd.Execute(product.Id);
            var result = cmd.Execute(product.Id, 2);

            // Assert

            AssertCommandSuccess(result);
            Assert.AreEqual(1, cart.Items.Count, "Добавился новый товар");
            Assert.AreEqual(3, cart.Items.First().Amount, "Количество товара не увеличилось");
        }
    }
}