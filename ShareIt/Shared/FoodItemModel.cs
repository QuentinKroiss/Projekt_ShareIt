
using System.ComponentModel.DataAnnotations;

namespace ShareIt.Shared
{
    //Kategorie = Auswahl für Artikel.
    public enum Category
    {
        Lebensmittel = 0,
        Getränke = 1,
        Objekte = 2
    }

    //Basismodell für Artikel die im Frontend genutzt werden.
    public class FoodItemModel
    {

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Category Category { get; set; }

        //MHD nur erfordert für Lebensmittel und Getränke, nicht für Objekte
        [RequiredIfFoodOrDrink(ErrorMessage = "MHD is required for Food or Drink category")]
        public DateTime? MHD { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
    }

    //Selbsterstelltes Validation Attribut für MHD je nach Kategorie
    public class RequiredIfFoodOrDrinkAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var foodItem = (FoodItemModel)validationContext.ObjectInstance;

            if (foodItem.Category == Category.Lebensmittel || foodItem.Category == Category.Getränke)
            {
                if (value == null || (value is DateTime date && date == default))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }

    // Erweiterung des Modells mit Datenbank-Id und Aufrufs-Zähler
    public class FoodItemModelWithId : FoodItemModel
    {
        public int Id { get; set; }
        public int Views { get; set; }
    }
}