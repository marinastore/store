using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class GetProductTest
    {
        /// <summary>
        /// Возвращается товар
        /// </summary>
        [TestMethod]
        public void Must_return_product()
        {
            var product = new Product
                          {
                              Name = "Флеш-память",
                              Description = "",
                              Category = new Category
                                         {
                                             Name = "Флешки"
                                         },
                              Price = 900,
                              Availability = ProductAvailability.Few
                          };
        }
    }
}