namespace Marina.Store.Web.Models
{
    public class CartItem
    {
        public CartItem() {}

        public CartItem(CartItem item)
        {
            Price = item.Price;
            ProductId = item.ProductId;
            Amount = item.Amount;
        }

        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set;  }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }
}