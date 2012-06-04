using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    // TODO: описать кейсы, связанные с самим логином (пока тут кейсы, связанные только с миграцией корзины)
    [TestClass]
    public class SignInTest
    {
        /// <summary>
        /// Если пользовател набрал корзину до логина
        /// Товары переносятся в корзину пользователя
        /// </summary>
        [TestMethod]
        public void When_there_is_session_cart_Must_put_items_to_user_cart_upon_sign_in()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Если пользовател набрал корзину до логина
        /// Она удаляется из сессии (т.к. дальше будет использоваться корзина пользователя)
        /// </summary>
        public void Must_delete_unsigned_session_cart_upon_sign_in()
        {
            Assert.Inconclusive();
        }
    }
}