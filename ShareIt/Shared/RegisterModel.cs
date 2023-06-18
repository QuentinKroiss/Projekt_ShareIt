using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShareIt.Shared
{
    public class RegisterModel
    {
        [Required, EmailAddress, Display(Name = "Email")]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password"),
            StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{6,}$",
            ErrorMessage = "The {0} must contain at least one digit and one special character.")]
        public string? Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm password"),
            Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
