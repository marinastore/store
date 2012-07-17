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

            AssertSuccess(result);
            Assert.AreEqual(product.Id, result.Value.Id, "Возвратился продукт с неверным Id");
            Assert.IsNotNull(result.Value.Category, "Продукт вернулся без категории");
            Assert.AreEqual(product.Name, result.Value.Name, "Неверное имя продукта");
            Assert.IsNotNull(result.Value.Params, "Продукт вернулся без параметров");
            Assert.AreEqual(product.Params.Count, result.Value.Params.Count, "Проукт вернулся с неверным числом параметров");
        }
    }
}