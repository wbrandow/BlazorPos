using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


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

        [Range(0, 100, ErrorMessage = "Discount must be 0 to 100.")]
        public decimal LineDiscount { get; set; } = 0;

        public bool Tax { get; set; } = true;

        public decimal GetLinePriceBeforeDiscount() {
            decimal linePrice = OrderProductPrice * QtyOnOrder;
            return linePrice;
        }

        public decimal GetLineDiscount() {
            decimal discount = OrderProductPrice * (LineDiscount / 100);
            return discount;
        }

        public decimal GetLinePrice() {
            decimal discountedPrice = OrderProductPrice - (OrderProductPrice * (LineDiscount / 100));
            return discountedPrice * QtyOnOrder;
        }
    }
}