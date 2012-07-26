using System.Collections.ObjectModel;
using System.Linq;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Models
{
    [TestClass]
    public class ShoppingCartTest
    {
        /// <summary>
        /// Подсчитывается цена всей корзины
        /// </summary>
        [TestMethod]
        public void Must_calculate_cart_total()
        {
            // Arrange

            var cart = new ShoppingCart();
            cart.Items.Add(new CartItem {Amount = 1, Price = 5});
            cart.Items.Add(new CartItem {Amount = 1, Price = 5});

            // Act

            var total = cart.Total;

            // Assert

            Assert.AreEqual(10, total, "Неверно считается цена корзины");
        }

        /// <summary>
        /// При подсчёте итоговой цены, учитывается кол-во единиц одного товара
        /// </summary>
        [TestMethod]
        public void When_calculating_cart_total_Must_consider_item_amounts()
        {
            // Arrange

            var cart = new ShoppingCart();
            cart.Items.Add(new CartItem { Amount = 1, Price = 5 });
            cart.Items.Add(new CartItem { Amount = 5, Price = 1 });

            // Act

            var total = cart.Total;

            // Assert

            Assert.AreEqual(10, total, "Неверно считается цена корзины");
        }

        /// <summary>
        /// Копии товаров переносятся между корзинами 
        /// (используется при миграции корзины)
        /// </summary>
        [TestMethod]
        public void Must_replace_items_with_new_ones()
        {
            // Arrange

            var cart = new ShoppingCart();
            cart.Items.Add(new CartItem { Id = 1 });
            var newItems = new Collection<CartItem> {new CartItem {Id = 2}, new CartItem {Id = 3}};

            // Act

            cart.ReplaceItems(newItems);

            // Assert

            Assert.AreEqual(2, cart.Items.Count, "Товары не перенеслись, либо перенеслись неправильно");
            Assert.IsTrue(cart.Items.All(i=>i.Id == 0), "Вместо копий, в корзину попали существующие товары");
        }
    }
}