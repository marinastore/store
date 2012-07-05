using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    // TODO: описать кейсы, связанные с самой регистрацией (пока тут кейсы, связанные только с миграцией корзины)
    [TestClass]
    public class RegisterTest
    {
        /// <summary>
        /// При регистрации создается новый пользователь
        /// </summary>
        [TestMethod]
        public void Must_create_user_upon_register()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Если пользователь уже зарегистрирован
        /// Возвращается ошибка
        /// </summary>
        [TestMethod]
        public void When_email_already_taken_Must_return_error()
        {
            Assert.Inconclusive();
        }

        
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
        /// Она удаляется из t (т.к. дальше будет использоваться корзина пользователя)
        /// </summary>
        public void Must_delete_unsigned_session_cart_upon_register()
        {
            Assert.Inconclusive();
        }
    }
}