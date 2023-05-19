using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShareIt.Shared
{
    public class DeleteUserModel
    {
        [Required, EmailAddress, Display(Name = "Email")]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string? Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
