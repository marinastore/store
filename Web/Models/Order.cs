using System;
using System.Collections.Generic;
using System.Linq;

namespace Marina.Store.Web.Models
{
    public class Order
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<OrderLine> Lines { get; set; }

        public Address Address { get; set; }

        public string Comment { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Total
        {
            get { return Lines == null ? 0 : Lines.Sum(l => l.Price * l.Amount); }
        }
    }
}