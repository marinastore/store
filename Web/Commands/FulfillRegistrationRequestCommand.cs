using System;
using System.Linq;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Infrastructure.Commands;
using Marina.Store.Web.Models;
using Marina.Store.Web.Policies;

namespace Marina.Store.Web.Commands
{
    public class FulfillRegistrationRequestCommand : Command
    {
        private readonly StoreDbContext _db;
        private readonly PasswordFormatPolicy _passwordPolicy;
        private readonly PasswordHashPolicy _passwordHashPolicy;

        public FulfillRegistrationRequestCommand(StoreDbContext db, PasswordFormatPolicy passwordPolicy, PasswordHashPolicy hashPolicy)
        {
            _db = db;
            _passwordPolicy = passwordPolicy;
            _passwordHashPolicy = hashPolicy;
        }

        public State IncorrectPasswordFormat;
        public State RegistrationRequestNotFound;
        public State EmailAlreadyRegistered;
        
        public Result Execute(Guid registrationRequestId, string password)
        {
            if (!_passwordPolicy.Check(password))
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

        private User CreateUser(RegistrationRequest request, string password)
        {
            string passwordHash = _passwordHashPolicy.Apply(password);

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

        #endregion
    }
}