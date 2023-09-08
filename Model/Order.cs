using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPos {
    /*
     *  represents a single order
     */
    public class Order {
        public int OrderId { get; set; }

        public DateTime CreatedTime { get; set; }

        public List<Product> Products { get; set; }

        public decimal GetTotalPrice() => Products.Sum(p => p.Price);
    }
}