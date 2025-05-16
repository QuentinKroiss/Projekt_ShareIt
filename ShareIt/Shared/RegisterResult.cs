
namespace ShareIt.Shared
{
    public class RegisterResult
    {
        // Ergebnis der Benutzerregistrierung.
        // Wird vom Server an den Client zurückgegeben, um über Erfolg oder Fehler zu informieren.
        public bool Successful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
