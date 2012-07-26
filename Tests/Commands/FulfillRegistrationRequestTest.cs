using System;
using System.Linq;
using Marina.Store.Web.Commands;
using Marina.Store.Web.Policies;
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
    public class FulfillRegistrationRequestTest : CommandTestBase
    {
        private const string PASSWORD = "qwerty";
        private const string HASH = "hash of qwerty";
        private Mock<PasswordFormatPolicy> _passwordFormatPolicy;
        private Mock<PasswordHashPolicy> _passwordHashPolicy;

        [TestInitialize]
        public void Init()
        {
            _passwordFormatPolicy = new Mock<PasswordFormatPolicy>();
            _passwordFormatPolicy.Setup(p => p.Check(It.IsAny<string>())).Returns(true);

            _passwordHashPolicy = new Mock<PasswordHashPolicy>();
            _passwordHashPolicy.Setup(p => p.Apply(It.IsAny<string>())).Returns(HASH);
        }

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

            var cmd = new FulfillRegistrationRequestCommand(Db, _passwordFormatPolicy.Object, _passwordHashPolicy.Object);
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
            _passwordFormatPolicy.Setup(p => p.Check(PASSWORD)).Returns(false);

            // Act

            var cmd = new FulfillRegistrationRequestCommand(Db, _passwordFormatPolicy.Object, _passwordHashPolicy.Object);
            var result = cmd.Execute(requestId, PASSWORD);

            // Assert

            AssertError(result, (FulfillRegistrationRequestCommand c)=>c.IncorrectPasswordFormat);
            _passwordFormatPolicy.Verify(p => p.Check(PASSWORD), Times.Exactly(1), "Пароль не проверяется");
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

            var cmd = new FulfillRegistrationRequestCommand(Db, _passwordFormatPolicy.Object, _passwordHashPolicy.Object);
            var result = cmd.Execute(request.Id, PASSWORD);
            Db.SaveChanges();

            // Assert

            AssertSuccess(result);
            var user = Db.Users.First(u => u.Email == request.Email);
            _passwordHashPolicy.Verify(p=>p.Apply(PASSWORD), Times.Exactly(1), "Не вызвана PasswordHashPolicy для генерации хэша");
            Assert.AreEqual(HASH, user.PasswordHash, "Хэш не сохранился");
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

            var cmd = new FulfillRegistrationRequestCommand(Db, _passwordFormatPolicy.Object, _passwordHashPolicy.Object);
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

            var cmd = new FulfillRegistrationRequestCommand(Db, _passwordFormatPolicy.Object, _passwordHashPolicy.Object);
            var result = cmd.Execute(request.Id, PASSWORD);
            Db.SaveChanges();

            // Assert
            
            AssertSuccess(result);
            Assert.IsTrue(Db.ShoppingCarts.Any(c=>c.User.Email == request.Email && c.Items.Any()), "Покупки не перенеслись в корзину пользователя");
        }

        /// <summary>
        /// После регистрации
        /// Пользователю отправляется email
        /// </summary>
        [TestMethod]
        public void When_registration_is_finished_Must_send_email()
        {
            Assert.Inconclusive();
        }
    }
}