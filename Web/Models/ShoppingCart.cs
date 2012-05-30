using System.Collections.Generic;
using System.Linq;

namespace Marina.Store.Web.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public ICollection<CartItem> Items { get; set; }

        public User User { get; set; }

        public decimal Total
        {
            get { return Items == null ? 0 : Items.Sum(l => l.Product.Price * l.Amount); }
        }

    }
}