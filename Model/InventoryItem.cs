using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace BlazorPos {

    /*
     *  Stores inventory information for a product
     */
    public class InventoryItem {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "QtyOnHand cannot be negative for an InventoryItem.")]
        public int QtyOnHand { get; set; }

        public decimal UnitCost { get; set; }

        public DateTime AquisitionDate { get; set; }

        public string AddedBy { get; set; }

        public int? PurchaseOrderId { get; set; }

    }
}