using Marina.Store.Web.Commands;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Commands
{
    [TestClass]
    public class AddToShoppingCartTest
    {
        public static int productId;

        [ClassInitialize]
        public static void Init(TestContext ctx)
        {
            using(var db = new StoreDbContext())
            {
                db.Database.ExecuteSqlCommand("delete from Users");
                db.Database.ExecuteSqlCommand("delete from Products");
                db.Database.ExecuteSqlCommand("delete from ShoppingCarts");

                var product = new Product
                              {
                                  Name = "Флеш",
                                  Vendor = "Россия",
                                  Price = 100,
                                  Availability = (int) ProductAvailability.Few,
                                  Category = new Category
                                             {
                                                 Name = "Флеш-носители" 
                                             }
                              };
                var user = new User
                           {
                               FirstName = "Вася",
                               LastName = "Обломов"
                           };

                db.Products.Add(product);
                db.Users.Add(user);
                db.SaveChanges();

                productId = product.Id;
            }            
        }

        /// <summary>
        /// Товар добавляется в корзину
        /// </summary>
        [TestMethod]
        public void Must_add_to_cart()
        {
            Assert.Inconclusive();
            using (var db = new StoreDbContext())
            {
              /*  var cmd = new AddToShoppingCartCommand(db);
                cmd.Execute(productId, 2);*/
            }
        }

        /// <summary>
        /// Если такой товар уже в корзине
        /// Увеличивается его кол-во (а не добавляется новый товар)
        /// </summary>
        [TestMethod]
        public void When_given_product_found_in_the_cart_Must_increment_amount()
        {
            Assert.Inconclusive();
        }
    }
}