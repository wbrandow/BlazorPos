using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPos {
    /*
     *  represents a single order
     */
    public class Sale {
        public int Id { get; set; }

        public string Salesperson { get; set; }

        public DateTime TimeOfSale { get; set; }

        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public List<SaleLine> SaleLines { get; set; } = new List<SaleLine>();

        public List<Payment> Payments { get; set; } = new List<Payment>();

        public bool Completed { get; set; }

        public decimal GetSubtotal() {
            decimal subtotal = 0;

            foreach (var line in SaleLines) {
                subtotal += line.GetLineSubtotal();
            }

            return subtotal;
        }

        public decimal GetTotalDiscount() {
            decimal totalDiscount = 0;

            foreach (var line in SaleLines) {
                totalDiscount += line.GetLineDiscount();
            }

            return totalDiscount;
        }

        public decimal GetDiscountedSubtotal() {
            decimal discountedSubtotal = 0;

            foreach (var line in SaleLines) {
                discountedSubtotal += line.GetLineTotal();
            }

            return discountedSubtotal;
        }

        public decimal GetTotalTax() {
            decimal totalTax = 0;

            foreach (var line in this.SaleLines) {
                if (line.ApplyTax == true) {
                    if (line.Product != null || line.Product.TaxClass != null) {
                        foreach (var taxRate in line.Product.TaxClass.TaxRates) {
                            if (taxRate != null) {
                                totalTax += line.GetLineTotal() * (taxRate.Rate / 100);
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

        public decimal GetTotal() {
            return this.GetDiscountedSubtotal() + this.GetTotalTax();
        }

        public decimal GetProfit() {
            decimal totalProfit = 0;

            foreach (var line in SaleLines) {
                totalProfit += line.GetLineProfit();
            }

            return totalProfit;
        }

        public int GetUnitsSold() {
            return this.SaleLines.Sum(line => line.Units);
        }
    }
}