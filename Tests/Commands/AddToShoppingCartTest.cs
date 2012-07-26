using System.Linq;
using Marina.Store.Web.Commands;
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
            var getCartMoq = MoqGetShoppingCart(cart);

            // Act

            var cmd = new AddToShoppingCartCommand(getCartMoq.Object);
            var result = cmd.Execute(product.Id);

            // Assert

            AssertSuccess(result);
            Assert.AreEqual(1, cart.Items.Count, "Товар не добавился в корзину"); 
            Assert.AreEqual(product.Id, cart.Items.First().ProductId, "Добавился не тот продукт");
            getCartMoq.Verify(c => c.Execute(GetShoppingCartCommand.FetchMode.GetOrCreate), Times.Exactly(1), "Комманда GetCart вызвана не в режиме GetOrDefault");
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

            AssertSuccess(result);
            Assert.AreEqual(1, cart.Items.Count, "Добавился новый товар");
            Assert.AreEqual(3, cart.Items.First().Amount, "Количество товара не увеличилось");
        }
    }
}