using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BlazorPos {
    public class User {
        public int Id { get; set; }

        [Required, MinLength(5, ErrorMessage = "Username must be at least 5 characters."), MaxLength(30, ErrorMessage = "Username cannot be longer than 30 characters")]
        public string Username { get; set; }

        [Required, MinLength(8, ErrorMessage = "Username must be at least 8 characters."), MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string Password { get; set; }
    }
}