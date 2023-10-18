using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace BlazorPos {
    /*
     *  Represents an individual tax rate (i.e. state sales tax, local sales tax, etc.)
     */
     public class TaxClass {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Tax Class name must be at least 2 characters."), MaxLength(50, ErrorMessage = "Input is too long.")]
        public string TaxClassName { get; set; }

        public List<TaxRate> TaxRates { get; set; } = new List<TaxRate>();
     }
}