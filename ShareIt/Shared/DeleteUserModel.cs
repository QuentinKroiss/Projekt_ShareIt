
using System.ComponentModel.DataAnnotations;


namespace ShareIt.Shared
{
    // Modell zur Validierung der Benutzereingaben beim Löschen eines Benutzerkontos.
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
