using System.Collections.Generic;
using Marina.Store.Web.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class GetShoppingCartTest : CommandTestBase
    {
        /// <summary>
        /// Для пользователей, у которых есть корзина
        /// Возвращается существующая
        /// </summary>
        [TestMethod]
        public void When_there_is_cart_for_user_Must_return_existing_cart()
        {
            // Arrange

            CreateEmptyCart();
            var user = CreateUser();
            var cart = CreateEmptyCart(user); // тестируемая корзина
            CreateEmptyCart();
            Db.SaveChanges();

            // Act

            var cmd = new GetShoppingCartCommand(Db, user);
            var result = cmd.Execute();

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.IsNotNull(result.Model, "Не вернулась корзина");
            Assert.AreEqual(1, Db.ShoppingCarts.Count(c => c.User.Id == user.Id), "Создалась новая корзина, либо уалилась старая");
            Assert.AreEqual(cart.Id, result.Model.Id, "Вернулась чужая корзина");
        }

        /// <summary>
        /// Для пользователей, у которых нет корзины, 
        /// Создается новая
        /// </summary>
        [TestMethod]
        public void When_there_is_no_cart_for_user_Must_create_new()
        {
            // Arrange

            CreateEmptyCart(); // просто корзина, не привязанная к пользователю
            var user = CreateUser();
            Db.SaveChanges();

            // Act

            var cmd = new GetShoppingCartCommand(Db, user);
            var result = cmd.Execute();

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.IsNotNull(result.Model, "Не вернулась корзина");
            Assert.AreEqual(user.Id, result.Model.User.Id, "Вернулась чужая корзина");
            Assert.AreEqual(1, Db.ShoppingCarts.Count(c => c.User.Id == user.Id), "Не создалась новая корзина, либо появилась лишняя");
        }


        /// <summary>
        /// Для незалогинненых пользователей
        /// Возвращается корзина, привязанная к текущей сессии
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_Must_return_existing_cart_for_session()
        {
            // Arrange

            var cart = CreateEmptyCart();
            Db.SaveChanges();
            var session = new Dictionary<string, object>();
            session[GetShoppingCartCommand.CART_SESSION_KEY] = cart.Id;

            // Act

            var cmd = new GetShoppingCartCommand(Db, null, session);
            var result = cmd.Execute();

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.IsNotNull(result.Model, "Не вернулась корзина");
            Assert.AreEqual(cart.Id, result.Model.Id, "Вернулась чужая корзина");
        }

        /// <summary>
        /// Для незалогинненых пользователей без корзины
        /// Создается новая корзина, ее id помещается в сессию
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_and_has_no_cart_Must_return_new_cart_and_store_id_in_session()
        {
            // Arrange

            var session = new Dictionary<string, object>();

            // Act

            var cmd = new GetShoppingCartCommand(Db, null, session);
            var result = cmd.Execute();

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.IsNotNull(result.Model, "Не вернулась корзина");
            Assert.AreEqual(result.Model.Id, session[GetShoppingCartCommand.CART_SESSION_KEY], "Id корзины не сохранился в сессию");
        }
    }
}