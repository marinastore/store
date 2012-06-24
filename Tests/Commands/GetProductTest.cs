using Marina.Store.Web.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class GetProductTest : CommandTestBase
    {
        /// <summary>
        /// Возвращается товар
        /// </summary>
        [TestMethod]
        public void Must_return_product()
        {
            // Arrange

            CreateProduct();
            var product = CreateProduct(); // тестируемый продукт
            CreateProduct();
            Db.SaveChanges();

            // Act

            var cmd = new GetProductCommand(Db);
            var result = cmd.Execute(product.Id);

            // Assert

            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
            Assert.AreEqual(product.Id, result.Model.Id, "Возвратился продукт с неверным Id");
            Assert.IsNotNull(result.Model.Category, "Продукт вернулся без категории");
            Assert.AreEqual(product.Name, result.Model.Name, "Неверное имя продукта");
            Assert.IsNotNull(result.Model.Params, "Продукт вернулся без параметров");
            Assert.AreEqual(product.Params.Count, result.Model.Params.Count, "Проукт вернулся с неверным числом параметров");
        }
    }
}