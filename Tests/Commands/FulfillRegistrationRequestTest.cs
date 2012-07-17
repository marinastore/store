using System;
using System.Linq;
using Marina.Store.Web.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    /// <summary>
    /// Процесс регистрации состоит из двух фаз:
    /// 1а. Создание заявки на регистрацию (AddRegistrationRequestTest)
    /// 1б. Письмо с подтверждением регистрации (IMailService.SendUserConfirmatiom)
    /// 2. Подтверждение регистрации (FulfillRegistrationRequestCommand)
    /// </summary>
    [TestClass]
    public class FulfillRegistrationRequestTest : CommandTestBase
    {
        private const string PASSWORD = "qwerty";


        /// <summary>
        /// При регистрации создается новый пользователь
        /// </summary>
        [TestMethod]
        public void Must_create_user_upon_register()
        {
            // Arrange

            var request = CreateRegistrationRequest();
            Db.SaveChanges();

            // Act

            var cmd = new FulfillRegistrationRequestCommand(Db);
            var result = cmd.Execute(request.Id, PASSWORD);
            Db.SaveChanges();

            // Assert

            AssertSuccess(result);
            Assert.AreEqual(1, Db.Users.Count(u => u.Email == request.Email), "Новый пользователь не создался, либо создалось несколько пользователей");
        }

        /// <summary>
        /// Пароль проверяется по формату
        /// </summary>
        [TestMethod]
        public void Must_validate_password()
        {
            // Arrange

            var requestId = Guid.NewGuid();

            // Act

            var cmd = new FulfillRegistrationRequestCommand(Db);
            var result = cmd.Execute(requestId, "1");

            // Assert

            AssertError(result, (FulfillRegistrationRequestCommand c)=>c.IncorrectPasswordFormat); 
            // TODO: задачка для самостоятельного решения: проверка нужна при регистрации и смене пароля, как организовать проверку пароля и тестирование?
        }

        /// <summary>
        /// С новым пользователем сохраняется хэш от его пароля
        /// </summary>
        [TestMethod]
        public void Must_store_password_hash_with_user()
        {
            // Arrange

            var request = CreateRegistrationRequest();
            Db.SaveChanges();

            // Act

            var cmd = new FulfillRegistrationRequestCommand(Db);
            var result = cmd.Execute(request.Id, PASSWORD);
            Db.SaveChanges();

            // Assert

            AssertSuccess(result);
            var user = Db.Users.First(u => u.Email == request.Email);
            Assert.IsNotNull(user.PasswordHash, "Хэш пароля не сохранился");
            Assert.AreNotEqual(PASSWORD, user.PasswordHash, "Сохранился сам пароль, а не его хэш");
        }

        /// <summary>
        /// Если пользователь уже зарегистрирован
        /// Возвращается ошибка
        /// </summary>
        [TestMethod]
        public void When_email_is_already_taken_Must_return_error()
        {
            // Arrange

            var request = CreateRegistrationRequest();
            var user = CreateUser();
            user.Email = request.Email; // занимаем email
            Db.SaveChanges();

            // Act

            var cmd = new FulfillRegistrationRequestCommand(Db);
            var result = cmd.Execute(request.Id, PASSWORD);

            // Assert 

            AssertError(result, (FulfillRegistrationRequestCommand c)=>c.EmailAlreadyRegistered);
        }

        
        /// <summary>
        /// Если пользовател набрал корзину до регистрации
        /// Товары переносятся в корзину пользователя
        /// </summary>
        [TestMethod]
        public void When_registration_request_has_cart_Must_put_items_to_user_cart_upon_register()
        {
            // Arrange

            var cart = CreateCart(items: CreateCartItem());
            var request = CreateRegistrationRequest(cart);
            Db.SaveChanges();

            // Act

            var cmd = new FulfillRegistrationRequestCommand(Db);
            var result = cmd.Execute(request.Id, PASSWORD);
            Db.SaveChanges();

            // Assert
            
            AssertSuccess(result);
            Assert.IsTrue(Db.ShoppingCarts.Any(c=>c.User.Email == request.Email && c.Items.Any()), "Покупки не перенеслись в корзину пользователя");
        }
    }
}