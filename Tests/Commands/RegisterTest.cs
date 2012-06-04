using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    // TODO: описать кейсы, связанные с самой регистрацией (пока тут кейсы, связанные только с миграцией корзины)
    [TestClass]
    public class RegisterTest
    {
        /// <summary>
        /// Если пользовател набрал корзину до регистрации
        /// Товары переносятся в корзину пользователя
        /// </summary>
        [TestMethod]
        public void When_there_is_session_cart_Must_put_items_to_user_cart_upon_register()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Если пользовател набрал корзину до регистрации
        /// Она удаляется из сессии (т.к. дальше будет использоваться корзина пользователя)
        /// </summary>
        public void Must_delete_unsigned_session_cart_upon_register()
        {
            Assert.Inconclusive();
        }
    }
}