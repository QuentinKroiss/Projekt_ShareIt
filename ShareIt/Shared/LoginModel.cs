
using System.ComponentModel.DataAnnotations;


namespace ShareIt.Shared
{
    // Datenmodell für die Benutzeranmeldung.
    // Enthält die erforderlichen Felder zur Authentifizierung eines Users.
    public class LoginModel
    {
        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
