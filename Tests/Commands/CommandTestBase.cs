using System;
using System.Globalization;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;

namespace Marina.Store.Tests.Commands
{
    /// <summary>
    /// Базовый класс для тестирования комманд.
    /// Заботится о том, чтобы все тесты запускались с чистой базой.
    /// Добавляет методы для генерации тестовых данных.
    /// </summary>
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
        /// Повторить действие N раз.
        /// Метод добавлен для простой генерации тестовых данных
        /// </summary>
        public void Repeat(Action action, int times)
        {
            for (var i =0; i < times; i++)
            {
                action();
            }
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
        /// Если категория не задана, каждый продукт создаётся в новой.
        /// </summary>
        public Product CreateProduct(Category category = null)
        {
            var num = _counter++;

            category = category ?? CreateCategory();

            var product = new Product
            {
                Name = "Тестовый продукт " + num,
                Description = "Описание тестового продукта",
                Vendor = "Производитель " + num,
                Price = (decimal) (99.99 + num),
                Availability = (int) ProductAvailability.Few,
                Category = category,
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
                FirstName = "Пользователь" + num,
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

        /// <summary>
        /// Создать категорию
        /// </summary>
        public Category CreateCategory()
        {
            var num = _counter++;

            var cat = new Category
            {
                Name = "Категория " + num
            };

            Db.Categories.Add(cat);

            return cat;
        }
    }
}