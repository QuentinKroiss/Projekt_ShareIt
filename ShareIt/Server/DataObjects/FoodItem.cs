using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ShareIt.Server.DataObjects
{
    public enum Category
    {
        Lebensmittel,
        Getränke,
        Objekte
    }

    public class FoodItem
    {
        private Category category;
        private DateTime? mhd;

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public Category Category
        {
            get { return category; }
            set
            {
                category = value;
                if (category != Category.Lebensmittel && category != Category.Getränke)
                {
                    MHD = null; // Wenn die Kategorie kein Lebensmittel oder Getränk ist, das MHD zurücksetzen
                }
            }
        }

        [RequiredIfFoodOrBeverage]
        public DateTime? MHD
        {
            get { return mhd; }
            set { mhd = value; }
        }

        [Required]
        public string Street { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        // Fremdschlüsselbeziehung zur AspNetUsers-Tabelle
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int Views { get; set; }
    }

    public class RequiredIfFoodOrBeverageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var foodItem = (FoodItem)validationContext.ObjectInstance;
            if ((foodItem.Category == Category.Lebensmittel || foodItem.Category == Category.Getränke) && value == null)
            {
                return new ValidationResult("Das Mindesthaltbarkeitsdatum (MHD) muss angegeben werden.");
            }

            return ValidationResult.Success;
        }
    }
}