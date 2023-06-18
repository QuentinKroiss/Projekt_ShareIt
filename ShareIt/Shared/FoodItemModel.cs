using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShareIt.Shared
{
    public enum Category
    {
        Lebensmittel,
        Getränke,
        Objekte
    }

    public class FoodItemModel
    {

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Category Category { get; set; }

        [RequiredIfFoodOrDrink(ErrorMessage = "MHD is required for Food or Drink category")]
        public DateTime? MHD { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        public int Views { get; set; }
    }

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

    public class FoodItemModelWithId : FoodItemModel
    {
        public int Id { get; set; }
    }
}