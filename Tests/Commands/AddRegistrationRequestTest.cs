using System;
using System.Linq;
using Marina.Store.Web.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Marina.Store.Tests.Commands
{
    /// <summary>
    /// Процесс регистрации состоит из двух фаз:
    /// 1а. Создание заявки на регистрацию (AddRegistrationRequestTest)
    /// 1б. Письмо с подтверждением регистрации (IMailService.SendUserConfirmatiom)
    /// 2. Подтверждение регистрации (FulfillRegistrationRequestCommand)
    /// </summary>
    [TestClass]
    public class AddRegistrationRequestTest : CommandTestBase
    {
        /// <summary>
        /// Заявка создается
        /// </summary>
        [TestMethod]
        public  void  Must_create_new_registration_request()
        {
            // Arrange

            var email = RandomEmail();

            // Act

            var cmd = new AddRegistrationRequestCommand(Db, MoqGetShoppingCart().Object, MoqMailService().Object);
            var result = cmd.Execute(email);
            Db.SaveChanges();

            // Assert

            AssertSuccess(result);
            var reqests = Db.RegistrationRequests.Where(r => r.Email == email).ToArray();
            Assert.IsTrue(reqests.Any(), "Заявка не добавилась");
            Assert.AreEqual(1, reqests.Count(), "Добавилась лишняя заявка");
        }


        /// <summary>
        /// Если емайл невалидный,
        /// Возвращается ошибка
        /// </summary>
        [TestMethod]
        public void When_email_is_not_valid_Must_return_error()
        {
            // Arrange

            const string email = "not valid email";

            // Act

            var cmd = new AddRegistrationRequestCommand(Db, MoqGetShoppingCart().Object, MoqMailService().Object);
            var result = cmd.Execute(email);

            // Assert
            
            AssertError(result, (AddRegistrationRequestCommand c)=>c.InvalidEmail);
        }

        /// <summary>
        /// Если существует пользователь с таким емайлом,
        /// Возвращается ошибка
        /// </summary>
        [TestMethod]
        public void When_email_is_taken_Must_return_error()
        {
            // Arrange

            var email = RandomEmail();
            var user = CreateUser();
            user.Email = email;
            Db.SaveChanges();

            // Act

            var cmd = new AddRegistrationRequestCommand(Db, MoqGetShoppingCart().Object, MoqMailService().Object);
            var result = cmd.Execute(email);

            // Assert

            AssertError(result, (AddRegistrationRequestCommand c) => c.EmailAlreadyRegistered);
        }

        /// <summary>
        /// Если у пользователя есть непустая корзина,
        /// К заявке добавляется ссылка на корзину 
        /// </summary>
        [TestMethod]
        public void When_user_has_non_empty_cart_Must_include_cart_reference()
        {
            // Arrange

            var email = RandomEmail();
            var nonEmptyCart = CreateCart(items: CreateCartItem());
            var getCartMoq = MoqGetShoppingCart(nonEmptyCart);
            Db.SaveChanges();

            // Act

            var cmd = new AddRegistrationRequestCommand(Db, getCartMoq.Object, MoqMailService().Object);
            var result = cmd.Execute(email);
            Db.SaveChanges();

            // Assert

            AssertSuccess(result);
            Assert.IsTrue(Db.RegistrationRequests.Any(r=>r.Email == email && r.ShoppingCartId == nonEmptyCart.Id), "Ссылка на непустую корзину не создалась");

            // Arrange 2 (пустая корзина)

            var email2 = RandomEmail();
            var getCartMoq2 = MoqGetShoppingCart();
            Db.SaveChanges();

            // Act 2 (пустая корзина)

            var cmd2 = new AddRegistrationRequestCommand(Db, getCartMoq2.Object, MoqMailService().Object);
            var result2 = cmd2.Execute(email2);
            Db.SaveChanges();

            // Assert 2 (пустая корзина)

            AssertSuccess(result2);
            Assert.IsTrue(Db.RegistrationRequests.Any(r => r.Email == email2 && r.ShoppingCartId == null), "Создалась ссылка на пустую корзину");
        }

        /// <summary>
        /// При создании заявки отсылается письмо для подтверждения емайла
        /// </summary> 
        [TestMethod]
        public void Must_send_email_confirmation()
        {
            // Arrange

            var email = RandomEmail();
            var mailMoq = MoqMailService();

            // Act

            var cmd = new AddRegistrationRequestCommand(Db, MoqGetShoppingCart().Object, mailMoq.Object);
            var result = cmd.Execute(email);

            // Assert

            AssertSuccess(result);
            mailMoq.Verify(s=>s.SendUserConfirmatiom(email, It.IsAny<string>()), Times.Exactly(1), "Письмо с подтверждением не отправлено");
        }


        #region Helpers

        private static string RandomEmail()
        {
            return string.Format("{0}@gmail.com", Guid.NewGuid().ToString("N"));
        }

        #endregion
    }
}