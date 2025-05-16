
namespace ShareIt.Shared
{
    public class DeleteUserResult
    {
        // Ergebnisobjekt für den Löschvorgang eines Benutzers.
        // Enthält Statusinformationen und mögliche Fehlermeldungen.
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
