using Marina.Store.Web.Infrastructure.Commands;
using Marina.Store.Web.Models;
using System.Linq;

namespace Marina.Store.Web.Commands
{
    public class AddToShoppingCartCommand : Command
    {
        private readonly GetShoppingCartCommand _getCartCmd;

        public State InvalidAmount;

        public AddToShoppingCartCommand(GetShoppingCartCommand getCartCmd)
        {
            _getCartCmd = getCartCmd;
        }

        public Result Execute(int productId, int amount = 1)
        {
            if (amount <= 0)
            {
                return Fail(() => InvalidAmount, "Указано некорректное количество товаров: " + amount);
            }

            // получаем корзину
            var cartResult = _getCartCmd.Execute();
            if (cartResult.HasErrors)
            {
                return cartResult.Outcome;
            }
            var cart = cartResult.Value;

            // добавляем товар
            // TODO: логику добавления товаров перенести в корзину
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

            return Ok();
        }
    }
}