using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Marina.Store.Web.Commands;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.MailService;
using Marina.Store.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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


        #region Asserts

        /// <summary>
        /// Проверить, что комманда успешно выполнена
        /// </summary>
        public void AssertCommandError(CommandResult result, string hasErrorsMessage = "Комманда выполнилась без ошибок")
        {
            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsTrue(result.HasErrors, hasErrorsMessage);
        }

        /// <summary>
        /// Проверить, что комманда выполнена с ошибками
        /// </summary>
        public void AssertCommandSuccess(CommandResult result)
        {
            Assert.IsNotNull(result, "Не возвратился результат");
            Assert.IsFalse(result.HasErrors, "Комманда выполнилась с ошибками");
        }

        #endregion


        #region Генерация тестовых данных

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
            _db.Database.ExecuteSqlCommand("delete from RegistrationRequests");
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
        /// Создать корзину
        /// </summary>
        public ShoppingCart CreateCart(User user = null, params CartItem[] items)
        {
            var cart = new ShoppingCart
            {
                User = user,
                Items = new Collection<CartItem>()
            };

            if (items != null)
            {
                foreach (var cartItem in items)
                {
                    cart.Items.Add(cartItem);
                }
            }

            Db.ShoppingCarts.Add(cart);

            return cart;
        }

        /// <summary>
        /// Создать покупку
        /// </summary>
        /// <returns></returns>
        public CartItem CreateCartItem(Product product = null, int amount = 1)
        {
            product = product ?? CreateProduct();

            var item = new CartItem
            {
                Amount = 1,
                Price = product.Price,
                Product = product
            };
            return item;
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

        /// <summary>
        /// Создать заявку на регистрацию
        /// </summary>
        public RegistrationRequest CreateRegistrationRequest(ShoppingCart cart = null)
        {
            var request = new RegistrationRequest
            {
                Id = Guid.NewGuid(),
                Email = string.Format("{0}@mail.ru", Guid.NewGuid().ToString("N")),
                Cart = cart
            };
            
            Db.RegistrationRequests.Add(request);

            return request;

        }

        #endregion


        #region Моки сервисов

        /// <summary>
        /// Создать mock сервиса отправки сообщений
        /// </summary>
        /// <returns></returns>
        public Mock<IMailService> MoqMailService()
        {
            var moq = new Mock<IMailService>();
            return moq;
        }

        /// <summary>
        /// Создать mock комманд получения корзины
        /// </summary>
        public Mock<GetShoppingCartCommand> MoqGetShoppingCart(ShoppingCart cart = null)
        {
            cart = cart ?? CreateCart();

            var moq = new Mock<GetShoppingCartCommand>(null, null, null);
            moq.Setup(c => c.Execute()).Returns(new CommandResult<ShoppingCart>(cart));

            return moq;
        }

        #endregion
    }
}