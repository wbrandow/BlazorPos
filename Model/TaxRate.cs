using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace BlazorPos {
    /*
     *  Represents an individual tax rate (i.e. state sales tax, local sales tax, etc.)
     */
     public class TaxRate {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Tax Name must be at least 2 characters."), MaxLength(50, ErrorMessage = "Input is too long.")]
        public string TaxRateName { get; set; }

        [Required, Range(1, 100, ErrorMessage = "Tax Rate must be 0 to 100.")]
        public decimal Rate { get; set; }
     }
}