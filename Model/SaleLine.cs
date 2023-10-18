using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace BlazorPos {

    /*
     *  Represents a sale line item
     */
    public class SaleLine {
        public int Id { get; set; }
        
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Units { get; set; }

        public decimal UnitCost { get; set; }

        public decimal UnitSalePrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be 0 to 100.")]
        public decimal LineDiscount { get; set; } = 0;

        public bool ApplyTax { get; set; } = true;

        public int SaleId { get; set; }

        [JsonIgnore]
        public Sale Sale { get; set; }
        

        public decimal GetLineSubtotal() {
            return Units * UnitSalePrice;
        }

        public decimal GetLineDiscount() {
            return Units * (UnitSalePrice * (LineDiscount / 100));
        }

        public decimal GetLineTotal() {
            return this.GetLineSubtotal() - this.GetLineDiscount();
        }

        public decimal GetLineProfit() {
            decimal lineCost = Units * UnitCost;
            return this.GetLineTotal() - lineCost;
        }
    }
}