using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Infrastructure.Commands;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.Commands
{
    public class FulfillRegistrationRequestCommand : Command
    {
        private readonly StoreDbContext _db;
        public const int MIN_PASSWORD_LENGTH = 4; 

        public FulfillRegistrationRequestCommand(StoreDbContext db)
        {
            _db = db;
        }

        public State IncorrectPasswordFormat;
        public State RegistrationRequestNotFound;
        public State EmailAlreadyRegistered;
        
        public Result Execute(Guid registrationRequestId, string password)
        {
            if (!CheckPasswordPolicy(password))
            {
                return Fail(()=>IncorrectPasswordFormat, "Неверный формат пароля");
            }

            var request = _db.RegistrationRequests.FirstOrDefault(r => r.Id == registrationRequestId);

            if (!CheckRequestExists(request))
            {
                return Fail(()=>RegistrationRequestNotFound, "Не существует заявки с id " + registrationRequestId);
            }

            if (!CheckEmailAvailible(request.Email))
            {
                return Fail(()=>EmailAlreadyRegistered, "Пользователь с таким емайлом существует " + request.Email);
            }

            var user = CreateUser(request, password);
            var cart = MigrateCart(request.ShoppingCartId, user);

            _db.Users.Add(user);
            if (cart != null)
            {
                _db.ShoppingCarts.Add(cart);
            }
            return Ok();
        }

        #region Private helpers

        private ShoppingCart MigrateCart(int? shoppingCartId, User user)
        {
            if (!shoppingCartId.HasValue)
            {
                return null;
            }

            var cart = _db.ShoppingCarts.FirstOrDefault(c=>c.Id == shoppingCartId);

            if (cart == null)
            {
                return null;
            }
            var newCart = new ShoppingCart
            {
                User = user
            };
            newCart.ReplaceItems(cart.Items);

            return newCart;
        }

        private static User CreateUser(RegistrationRequest request, string password)
        {
            string passwordHash;
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                // TODO: использовать соль!
                passwordHash = Encoding.UTF8.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash
            };

            return user;
        }

        private bool CheckEmailAvailible(string email)
        {
            return !_db.Users.Any(u => u.Email == email);
        }

        private static bool CheckRequestExists(RegistrationRequest request)
        {
            return request != null;
        }

        private static bool CheckPasswordPolicy(string password)
        {
            return password != null && password.Length >= MIN_PASSWORD_LENGTH;
        }

        #endregion
    }
}