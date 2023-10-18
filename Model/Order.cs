/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlazorPos {
    /*
     *  represents a single order
     *
    public class Order {
        public int OrderId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        public decimal GetTotalPriceBeforeDiscount() {
            decimal price = 0;
            decimal linePrice;

            foreach (var orderProduct in OrderProducts) {
                linePrice = orderProduct.GetLinePriceBeforeDiscount();
                price += linePrice;
            }

            return price;
        }

        public decimal GetTotalDiscount() {
            decimal totalDiscount = 0;
            decimal lineDiscount;

            foreach (var orderProduct in OrderProducts) {
                lineDiscount = orderProduct.GetLineDiscount();
                totalDiscount += lineDiscount;
            }

            return totalDiscount;
        }

        public decimal GetTotalPrice() {
            decimal totalPrice = 0;
            decimal linePrice;
            
            foreach (var orderProduct in OrderProducts) {
                linePrice = orderProduct.GetLinePrice();
                totalPrice += linePrice;
            }

            return totalPrice;
        }

        public decimal GetTotalTax() {
            decimal totalTax = 0;
            
            foreach (var orderProduct in OrderProducts) {

                if (orderProduct.Tax == true) {
                    if (orderProduct.Product != null && orderProduct.Product.TaxClass != null) {
                        foreach (var taxRate in orderProduct.Product.TaxClass.TaxRates) {
                            if (taxRate != null) {
                                totalTax += orderProduct.GetLinePrice() * (taxRate.Rate / 100);
                            }
                        }
                    }
                    else {
                        Console.WriteLine("TAX CLASS IS NULL!!!!!!!!!!");
                    }
                }
            }

            return totalTax;
        }

        public decimal GetTotalPriceWithTax() {
            return GetTotalPrice() + GetTotalTax();
        }
    }
}
*/