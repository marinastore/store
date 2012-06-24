using System.Collections.Generic;
using Marina.Store.Web.Commands;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class GetShoppingCartTest
    {
        public static int anonCartId;
        public static int userCartId;

        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            using (var db = new StoreDbContext())
            {
                db.Database.ExecuteSqlCommand("delete from Users");
                db.Database.ExecuteSqlCommand("delete from ShoppingCarts");
                db.SaveChanges();
            }

            var user1 = new User
            {
                FirstName = "Вася",
                LastName = "С корзиной"
            };

            var user2 = new User
            {
                FirstName = "Вася",
                LastName = "Без корзины"
            };

            var cart = new ShoppingCart
            {
                User = user1
            };

            var anonCart = new ShoppingCart();

            using (var db = new StoreDbContext())
            {
                db.Users.Add(user2);
                db.ShoppingCarts.Add(cart);
                db.ShoppingCarts.Add(anonCart);
                db.SaveChanges();
            }

            anonCartId = anonCart.Id;
            userCartId = cart.Id;
        }


        /// <summary>
        /// Для пользователей, у которых есть корзина
        /// Возвращается существующая
        /// </summary>
        [TestMethod]
        public void When_there_is_cart_for_user_Must_return_existing_cart()
        {
            using ( var db = new StoreDbContext())
            {
                var user = db.Users.First(u => u.LastName == "С корзиной");
                var cmd = new GetShoppingCartCommand(db, user);
                var result = cmd.Execute();
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);

                Assert.AreEqual(1, db.ShoppingCarts.Count(c => c.User.Id == user.Id), "Если у пользователя есть корзина, новая не добавляется");

                Assert.AreEqual(userCartId, result.Model.Id);
            }
        }

        /// <summary>
        /// Для пользователей, у которых нет корзины, 
        /// Создается новая
        /// </summary>
        [TestMethod]
        public void When_there_is_no_cart_for_user_Must_create_new()
        {
            using (var db = new StoreDbContext())
            {
                var user2 = db.Users.First(u => u.LastName == "Без корзины");
                var cmd = new GetShoppingCartCommand(db, user2);
                var result = cmd.Execute();
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                Assert.AreEqual(user2.Id, result.Model.User.Id);
                Assert.AreEqual(1, db.ShoppingCarts.Count(c => c.User.Id == user2.Id));
            }
        }


        /// <summary>
        /// Для незалогинненых пользователей
        /// Возвращается корзина, привязанная к текущей сессии
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_Must_return_existing_cart_for_session()
        {
            var session = new Dictionary<string, object>();
            session[GetShoppingCartCommand.CART_SESSION_KEY] = anonCartId;
            using (var db = new StoreDbContext())
            {
                var cmd = new GetShoppingCartCommand(db, null, session);
                var result = cmd.Execute();
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);

                Assert.AreEqual(anonCartId, result.Model.Id);
            }
        }

        /// <summary>
        /// Для незалогинненых пользователей без корзины
        /// Создается новая корзина, ее id помещается в сессию
        /// </summary>
        [TestMethod]
        public void When_user_is_not_signed_in_and_has_no_cart_Must_return_new_cart_and_store_id_in_session()
        {
            var session = new Dictionary<string, object>();
            using (var db = new StoreDbContext())
            {
                var cmd = new GetShoppingCartCommand(db, null, session);
                var result = cmd.Execute();
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                Assert.AreEqual(result.Model.Id, session[GetShoppingCartCommand.CART_SESSION_KEY]);
            }
        }
    }
}