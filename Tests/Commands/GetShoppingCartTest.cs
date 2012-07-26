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
        /// В режие Get, для пользователей, у которых есть корзина
        /// Возвращается существующая или null, если корзины нет
        /// </summary>
        [TestMethod]
        public void When_there_is_cart_for_user_Must_return_existing_cart()
        {
            // Arrange, случай, если корзина есть 

            CreateCart();
            var user = CreateUser();
            var cart = CreateCart(user); // тестируемая корзина
            CreateCart();
            Db.SaveChanges();

            // Act

            var cmd = new GetShoppingCartCommand(Db, user);
            var result = cmd.Execute(GetShoppingCartCommand.FetchMode.Get);

            // Assert

            AssertSuccess(result);
            Assert.IsNotNull(result.Value, "Не вернулась корзина");
            Assert.AreEqual(1, Db.ShoppingCarts.Count(c => c.User.Id == user.Id), "Создалась новая корзина, либо удалилась старая");
            Assert.AreEqual(cart.Id, result.Value.Id, "Вернулась чужая корзина");

            // Arrange2, случай, если корзины нет

            var user2 = CreateUser();
            Db.SaveChanges();

            // Act2

            var cmd2 = new GetShoppingCartCommand(Db, user2);
            var result2 = cmd2.Execute(GetShoppingCartCommand.FetchMode.Get);

            // Assert2
            AssertSuccess(result2);
            Assert.IsNull(result2.Value, "Не вернулся null, если нет корзины");
        }

        /// <summary>
        /// В режие getOrCreate для пользователей, у которых нет корзины 
        /// Создается новая
        /// </summary>
        [TestMethod]
        public void When_there_is_no_cart_for_user_Must_create_new()
        {
            // Arrange

            CreateCart(); // просто корзина, не привязанная к пользователю
            var user = CreateUser();
            Db.SaveChanges();

            // Act

            var cmd = new GetShoppingCartCommand(Db, user);
            var result = cmd.Execute(GetShoppingCartCommand.FetchMode.GetOrCreate);

            // Assert

            AssertSuccess(result);
            Assert.IsNotNull(result.Value, "Не вернулась корзина");
            Assert.AreEqual(user.Id, result.Value.User.Id, "Вернулась чужая корзина");
            Assert.AreEqual(1, Db.ShoppingCarts.Count(c => c.User.Id == user.Id), "Не создалась новая корзина, либо появилась лишняя");
        }


        /// <summary>
        /// В режиме Get, для незалогинненых пользователей
        /// Возвращается корзина, привязанная к текущей сессии
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_Must_return_existing_cart_for_session()
        {
            // Arrange, в случае, если корзина существует

            var cart = CreateCart();
            Db.SaveChanges();
            var session = new Dictionary<string, object>();
            session[GetShoppingCartCommand.CART_SESSION_KEY] = cart.Id;

            // Act

            var cmd = new GetShoppingCartCommand(Db, null, session);
            var result = cmd.Execute(GetShoppingCartCommand.FetchMode.Get);

            // Assert

            AssertSuccess(result);
            Assert.IsNotNull(result.Value, "Не вернулась корзина");
            Assert.AreEqual(cart.Id, result.Value.Id, "Вернулась чужая корзина");

            // Arrange2, в случае, если корзины не существует
            
            var emptySession = new Dictionary<string, object>();

            // Act2

            var cmd2 = new GetShoppingCartCommand(Db, null, emptySession);
            var result2 = cmd2.Execute(GetShoppingCartCommand.FetchMode.Get);

            // Assert2

            AssertSuccess(result2);
            Assert.IsNull(result2.Value, "Не вернулся null, если нет корзины");
        }

        /// <summary>
        /// В режиме GetOrDefault для незалогинненых пользователей без корзины
        /// Создается новая корзина, ее id помещается в сессию
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_and_has_no_cart_Must_return_new_cart_and_store_id_in_session()
        {
            // Arrange

            var session = new Dictionary<string, object>();

            // Act

            var cmd = new GetShoppingCartCommand(Db, null, session);
            var result = cmd.Execute(GetShoppingCartCommand.FetchMode.GetOrCreate);

            // Assert

            AssertSuccess(result);
            Assert.IsNotNull(result.Value, "Не вернулась корзина");
            Assert.AreEqual(result.Value.Id, session[GetShoppingCartCommand.CART_SESSION_KEY], "Id корзины не сохранился в сессию");
        }
    }
}