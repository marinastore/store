using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;
using System.Linq;

namespace Marina.Store.Web.Commands
{
    public class AddToShoppingCartCommand : Command
        {
            private readonly StoreDbContext _db;
            private readonly GetShoppingCartCommand _getCartCmd;

            public AddToShoppingCartCommand(StoreDbContext db, GetShoppingCartCommand getCartCmd)
            {
                _db = db;
                _getCartCmd = getCartCmd;
            }


            public CommandResult Execute(int productId, int amount = 1)
            {
                if (amount <= 0)
                {
                    return Fail("amount", "Укажите количество товара.");
                }

                var cart = _getCartCmd.Execute().Model;
                var item = cart.Items.FirstOrDefault(c => c.ProductId == productId);
                if (item == null)
                {
                    item = new CartItem
                    {
                        ProductId = productId,
                    };
                    cart.Items.Add(item);
                }
                item.Amount += amount;

                return Success();
            }
        }
}