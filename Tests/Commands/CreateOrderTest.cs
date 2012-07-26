using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class CreateOrderTest: CommandTestBase
    {
        /// <summary>
        /// Заказ должен оформляться
        /// </summary>
        [TestMethod]
        public void Must_create_order()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// При оформлении заказа
        /// Валидируется адресс
        /// </summary>
        [TestMethod]
        public void Must_validate_address()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// После создания заказа для залогиненого пользователя
        /// Ему отсылается email
        /// </summary>
        [TestMethod]
        public void When_order_created_for_existing_user_Must_send_email()
        {
            Assert.Inconclusive();
        }
    }
}