using System.Globalization;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;

namespace Marina.Store.Tests.Commands
{
    public class CommandTestBase
    {
        private readonly StoreDbContext _db;
        private int _counter;

        public StoreDbContext Db
        {
            get { return _db; }
        }

        public CommandTestBase()
        {
            _db = new StoreDbContext();

            // Каждый тест выполняется на пустой базе данных
            CleanUpDatabase();
        }

        /// <summary>
        /// Удалить все данные из базы
        /// </summary>
        public void CleanUpDatabase()
        {
            _db.Database.ExecuteSqlCommand("delete from CartItems");
            _db.Database.ExecuteSqlCommand("delete from ShoppingCarts");
            _db.Database.ExecuteSqlCommand("delete from OrderLines");
            _db.Database.ExecuteSqlCommand("delete from Orders");
            _db.Database.ExecuteSqlCommand("delete from Addresses");
            _db.Database.ExecuteSqlCommand("delete from Params");
            _db.Database.ExecuteSqlCommand("delete from Products");
            _db.Database.ExecuteSqlCommand("delete from Categories");
            _db.Database.ExecuteSqlCommand("delete from Users");
        }

        /// <summary>
        /// Создать тестовый продукт c двумя параметрами.
        /// Каждый продукт создаётся в новой категории.
        /// </summary>
        public Product CreateProduct()
        {
            var num = _counter++;

            var product = new Product
            {
                Name = "Тестовый продукт " + num,
                Description = "Описание тестового продукта",
                Vendor = "Производитель " + num,
                Price = (decimal) (99.99 + num),
                Availability = (int) ProductAvailability.Few,
                Category = new Category
                {
                    Name = "Тестовая категория " + num,
                    Description = "Описание тестовой категории" + num
                },
                Params = new[]
                {
                    new Param
                    {
                        Name = "Размер",
                        Value = num + "гб"
                    },
                    new Param
                    {
                        Name = "Цвет",
                        Value = "Красный"
                    }
                }
            };

            Db.Products.Add(product);

            return product;
        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        public User CreateUser()
        {
            var num = _counter++;

            var user = new User
            {
                FirstName = "Пользователь",
                LastName = num.ToString(CultureInfo.InvariantCulture)
            };

            Db.Users.Add(user);

            return user;
        }

        /// <summary>
        /// Создать пустую корзину
        /// </summary>
        public ShoppingCart CreateEmptyCart(User user = null)
        {
            var cart = new ShoppingCart { User = user };

            Db.ShoppingCarts.Add(cart);

            return cart;
        }
    }
}