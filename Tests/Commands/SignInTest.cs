using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class SignInTest : CommandTestBase
    {
        /// <summary>
        /// При успешной аутентификации 
        /// Возвращается пользователь
        /// </summary>
        [TestMethod]
        public void When_authentication_successfull_Must_return_user()
        {
            Assert.Inconclusive();    
        }

        /// <summary>
        /// При аутентификации проверяется пароль
        /// </summary>
        [TestMethod]
        public void When_authenticating_Must_verify_password()
        {
            Assert.Inconclusive();
        }

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
        [TestMethod]
        public void Must_delete_unsigned_session_cart_upon_sign_in()
        {
            Assert.Inconclusive();
        }
    }
}