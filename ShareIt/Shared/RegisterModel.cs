using System.ComponentModel.DataAnnotations;


namespace ShareIt.Shared
{
    // Datenmodell zur Benutzerregistrierung.
    // Wird vom Client an den Server gesendet, um einen neuen Benutzeraccount zu erstellen.
    public class RegisterModel
    {
        // Muss ein gültiges E-Mail-Format haben, als Bsp. 'abc@gmx.de' oder '123@gmail.com'.
        [Required, EmailAddress, Display(Name = "Email")]
        public string? Email { get; set; }

        // Das gewünschte Passwort des Benutzers.
        // Muss mindestens 6 Zeichen lang sein, mindestens eine Ziffer und ein Sonderzeichen enthalten.
        [Required, DataType(DataType.Password), Display(Name = "Password"),
            StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{6,}$",
            ErrorMessage = "The {0} must contain at least one digit and one special character.")]
        public string? Password { get; set; }

        // Bestätigung des Passworts.
        // Muss identisch mit dem ursprünglichen Passwort sein.
        [Required, DataType(DataType.Password), Display(Name = "Confirm password"),
            Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
