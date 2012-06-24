using System;

namespace Marina.Store.Web.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set;  }

        public int Amount { get; set; }
    }
}