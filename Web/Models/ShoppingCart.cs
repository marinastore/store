using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void ReplaceItems(ICollection<CartItem> items)
        {
            if (Items != null)
            {
                Items.Clear();
            }
            else
            {
                Items = new Collection<CartItem>();
            }

            foreach(var item in items)
            {
                Items.Add(new CartItem(item));
            }
        }

    }
}