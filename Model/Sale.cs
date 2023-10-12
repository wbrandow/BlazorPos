using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPos {
    /*
     *  represents a single order
     */
    public class Sale {
        public int Id { get; set; }

        public string SoldBy { get; set; }

        public DateTime SoldTime { get; set; }

        public List<InventoryItem> SoldItems { get; set; }
    }
}