using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPos {
    /*
     *  represents a single payment
     */
    public class Payment {
        public int Id { get; set; }
        
        public string PaymentType { get; set; }

        public decimal Amount { get; set; }

        public int SaleId { get; set; }
    }
}