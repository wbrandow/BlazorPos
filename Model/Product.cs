using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorPos {
    /*
     *  Represents a single item
     */
    public class Product {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Input is too long.")]
        public string UPC { get; set; }

        [MaxLength(50, ErrorMessage = "Input is too long.")]
        public string EAN { get; set; }

        [MaxLength(50, ErrorMessage = "Input is too long.")]
        public string SKU { get; set; }

        [Required, MinLength(3, ErrorMessage = "Description must be at least 3 characters."), MaxLength(100, ErrorMessage = "Input is too long.")]
        public string Description { get; set; }

        [MinLength(3, ErrorMessage = "Brand must be at least 3 characters."), MaxLength(50, ErrorMessage = "Input is too long.")]
        public string Brand { get; set; }

        [MinLength(3, ErrorMessage = "Vendor must be at least 3 characters."), MaxLength(50, ErrorMessage = "Input is too long.")]
        public string Vendor { get; set; }

        [Required, RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal Price { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal MSRP { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal OnlinePrice { get; set; }        

        [RegularExpression(@"^\d+(\.\d{1,2})?$?", ErrorMessage = "Please enter a valid price.")]
        public decimal DefaultCost { get; set; }

        public int TaxClassId { get; set; } = 1;

        public TaxClass TaxClass { get; set; }

        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();


        public int GetQuantityOnHand() {
            int qtyOnHand = 0;

            foreach (var inventoryItem in InventoryItems) {
                qtyOnHand += inventoryItem.QtyOnHand;
            }

            return qtyOnHand;
        }
    } 
}