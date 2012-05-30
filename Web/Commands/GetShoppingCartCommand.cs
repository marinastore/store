using System.Linq;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.Commands
{
    public class GetShoppingCartCommand : Command
    {
        private readonly StoreDbContext _db;
        private readonly User _user;

        public GetShoppingCartCommand(StoreDbContext db, User user)
        {
            _db = db;
            _user = user;
        }

        public CommandResult<ShoppingCart> Execute()
        {
            var cart = _db.ShoppingCarts.Include("Items").First(c => c.User.Id == _user.Id);
            return Result(cart);
        }
    }
}