using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class GetShoppingCartTest
    {
        /// <summary>
        /// Для пользователей, у которых есть корзина
        /// Возвращается существующая
        /// </summary>
        [TestMethod]
        public void When_there_is_cart_for_user_Must_return_existing_cart()
        {
            Assert.Inconclusive();
        }
        
        /// <summary>
        /// Для незалогинненых пользователей
        /// Возвращается корзина, привязанная к текущей сессии
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_Must_return_existing_cart_for_session()
        {
            Assert.Inconclusive();
        }
    }
}