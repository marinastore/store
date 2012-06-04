using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class AddToShoppingCartTest
    {
        /// <summary>
        /// Товар добавляется в карзину
        /// </summary>
        [TestMethod]
        public void Must_add_to_cart()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Если такой товар уже в корзине
        /// Увеличивается его кол-во (а не добавляется новый товар)
        /// </summary>
        [TestMethod]
        public void When_given_product_found_in_the_cart_Must_increment_amount()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Для пользователей, у которых ещё нет корзины
        /// Создаётся новая, в которую добавляется товар
        /// </summary>
        [TestMethod]
        public void When_there_is_no_cart_for_user_Must_create_new_and_add_item()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Для незалогиненных пользователей, у которых ещё нет корзины
        /// Создаётся и привязывается к сессии новая, в которую добавляется товар
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_and_has_no_cart_Must_create_new_and_add_item()
        {
            Assert.Inconclusive();
        }
    }
}