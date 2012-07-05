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
        private ShoppingCart _cart;
        private Mock<GetShoppingCartCommand> _getCartMoq;

        /// <summary>
        /// Подделываем комманду получения корзины перед каждым тестом
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _cart = CreateEmptyCart();
            _getCartMoq = new Mock<GetShoppingCartCommand>(null, null, null);
            _getCartMoq.Setup(c => c.Execute()).Returns(new CommandResult<ShoppingCart>(_cart));  
        }
        
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
            Db.SaveChanges();

            // Act

            var cmd = new AddToShoppingCartCommand(_getCartMoq.Object);
            var result = cmd.Execute(product.Id);

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.AreEqual(1, _cart.Items.Count, "Товар не добавился в корзину"); 
            Assert.AreEqual(product.Id, _cart.Items.First().ProductId, "Добавился не тот продукт");
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
            Db.SaveChanges();

            // Act

            var cmd = new AddToShoppingCartCommand(_getCartMoq.Object);
            cmd.Execute(product.Id);
            var result = cmd.Execute(product.Id, 2);

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.AreEqual(1, _cart.Items.Count, "Добавился новый товар");
            Assert.AreEqual(3, _cart.Items.First().Amount, "Количество товара не увеличилось");
        }
    }
}