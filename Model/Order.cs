using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPos {
    /*
     *  represents a single order
     */
    public class Order {
        public int OrderId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public decimal GetTotalPrice() {
            decimal totalPrice = 0;
            decimal linePrice;
            
            foreach (var orderProduct in OrderProducts) {
                linePrice = orderProduct.GetLinePrice();
                totalPrice += linePrice;
            }

            return totalPrice;
        }
    }
}