using System.Collections.Generic;
using System.Linq;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Infrastructure.Commands;
using Marina.Store.Web.Models;
using System.Data.Entity;

namespace Marina.Store.Web.Commands
{
    public class GetShoppingCartCommand : Command
    {
        private readonly StoreDbContext _db;
        private readonly User _user;
        private readonly IDictionary<string,object> _session;
        public const string CART_SESSION_KEY = "shoppingCartId";

        public GetShoppingCartCommand(StoreDbContext db, User user, IDictionary<string, object> session = null)
        {
            _db = db;
            _user = user;
            _session = session;
        }

        public virtual Result<ShoppingCart> Execute()
        {
            if (_user == null)
            {
                return GetSessionCart() ?? CreateSessionCart(_session);
            }

            return GetUserCart() ?? CreateUserCart(_user);
        }

        #region Private helpers

        private ShoppingCart GetUserCart()
        {
            return _db.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.User.Id == _user.Id);
        }

        private ShoppingCart CreateUserCart(User user)
        {
            var cart = new ShoppingCart
            {
                User = user
            };
            _db.ShoppingCarts.Add(cart);
            _db.SaveChanges();
            return cart;
        }

        private ShoppingCart GetSessionCart()
        {
            if (!_session.ContainsKey(CART_SESSION_KEY))
            {
                return null;
            }

            var id = _session[CART_SESSION_KEY] as int?;
            return _db.ShoppingCarts.FirstOrDefault(c => c.Id == id);          
        }

        private ShoppingCart CreateSessionCart(IDictionary<string, object> session)
        {
            var cart = new ShoppingCart();
            _db.ShoppingCarts.Add(cart);
            _db.SaveChanges();
            session[CART_SESSION_KEY] = cart.Id;
            return cart;
        }

        #endregion
    }
}