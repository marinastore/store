using System;
using System.Linq;

namespace Marina.Store.Web.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }

        public CartLine[] Lines { get; set; }

        public decimal Total
        {
            get { return Lines.Sum(l => l.Product.Price); }
        }

    }
}