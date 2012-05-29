namespace Marina.Store.Web.Models
{
    public class OrderLine
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string Amount { get; set; }

        public decimal Price { get; set; }

        public Product Product { get; set; }
    }
}