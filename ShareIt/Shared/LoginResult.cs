
namespace ShareIt.Shared
{
    public class LoginResult
    {
        // Repräsentiert das Ergebnis eines Login-Vorgangs.
        // Wird vom Server zurückgegeben, um den Status der Authentifizierung mitzuteilen.
        public bool Successful { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
    }
}
