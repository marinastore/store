using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class ChangeItemAmountTest : CommandTestBase
    {
        /// <summary>
        /// Кол-во единиц товара должно меняться
        /// </summary>
        [TestMethod]
        public void Must_change_item_amount()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Если продукта нет в корзине
        /// Возвращается ошибка
        /// </summary>
        [TestMethod]
        public void When_item_not_found_Must_return_error()
        {
            Assert.Inconclusive();
        }
    }
}