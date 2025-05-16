
namespace ShareIt.Shared
{
    public class FoodItemResult
    {
        // Repräsentiert das Ergebnis eines Vorgangs im Zusammenhang mit einem Artikel (FoodItem).
        // Enthält Statusinformationen sowie mögliche Fehlermeldungen.
        public bool Successful { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
