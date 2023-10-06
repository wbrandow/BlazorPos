using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;


namespace BlazorPos {

    /*
     *  This is a junction between Order and Product and can be thought
     *  of as a line item on an Order.
     */
    public class OrderProduct {
        public int Id { get; set; }
        
        public int OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int QtyOnOrder { get; set; }

        public decimal OrderProductPrice { get; set; }

        public decimal LineDiscount { get; set; }

        public bool Tax { get; set; } = true;

        public decimal GetLinePrice() {
            return OrderProductPrice * QtyOnOrder;
        }
    }
}