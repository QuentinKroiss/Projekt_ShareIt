using System.ComponentModel.DataAnnotations;

namespace ShareIt.Shared
{
    public class FoodItemModel
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public string UserName { get; set; }

        public string ImageUrl { get; set; } // Hinzufügen des ImageUrl-Feldes
    }
}
