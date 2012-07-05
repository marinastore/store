using System.Linq;
using Marina.Store.Web.Commands;
using Marina.Store.Web.MailService;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class CreateRegistrationRequestTest : CommandTestBase
    {
        private ShoppingCart _cart;
        private Mock<GetShoppingCartCommand> _getCartMoq;
        private Mock<IMailService> _mailService;

        /// <summary>
        /// Подделываем зависимости
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _cart = CreateEmptyCart();
            _getCartMoq = new Mock<GetShoppingCartCommand>(null, null, null);
            _getCartMoq.Setup(c => c.Execute()).Returns(new CommandResult<ShoppingCart>(_cart));
            _mailService = new Mock<IMailService>();
        }
   
        /// <summary>
        /// Заявка создается
        /// </summary>
        [TestMethod]
        public  void  Must_create_new_registration_request()
        {
            // Arrange

            const string email = "mail@gmail.com";

            // Act

            var cmd = new CreateRegistrationRequestCommand(Db, _getCartMoq.Object, _mailService.Object);
            var result = cmd.Execute(email);
            Db.SaveChanges();

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.IsTrue(Db.RegistrationRequests.Any(), "Заявка не добавилась");
            Assert.AreEqual(1, Db.RegistrationRequests.Count(), "Добавилась лишняя заявка"); // TODO: исправить метод проверки
        }


        /// <summary>
        /// Если емайл невалидный,
        /// Возвращается ошибка
        /// </summary>
        [TestMethod]
        public void When_email_is_not_valid_Must_return_error()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Если существует пользователь с таким емайлом,
        /// Возвращается ошибка
        /// </summary>
        [TestMethod]
        public void When_email_is_taken_Must_return_error()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Если у пользователя есть непустая корзина,
        /// К заявке добавляется ссылка на корзину 
        /// </summary>
        [TestMethod]
        public void When_user_has_non_empty_cart_Must_include_cart_reference()
        {
            
        }

        /// <summary>
        /// При создании заявки отсылается письмо для подтверждения емайла
        /// </summary> 
        [TestMethod]
        public void Must_send_email_confirmation()
        {
            
        }


    }
}