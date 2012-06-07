using System.Collections.Generic;

namespace Marina.Store.Web.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Vendor { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public int Availability { get; set; }

        public ICollection<Param> Params { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

    }
}