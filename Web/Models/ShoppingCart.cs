using System.Collections.Generic;
using System.Linq;

namespace Marina.Store.Web.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public ICollection<CartItem> Items { get; set; }

        public decimal Total
        {
            get { return Items.Sum(l => l.Product.Price); }
        }

    }
}