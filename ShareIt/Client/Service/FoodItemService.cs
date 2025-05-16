using Blazored.LocalStorage;
using ShareIt.Shared;
using System.Net.Http.Json;

namespace ShareIt.Client.Service
{
    //Serviceklasse für das Hinzuüfgen eines neuen Artikels (FoodItem)
    public class FoodItemService : IFoodItemService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private List<FoodItemModel> foodItems = new List<FoodItemModel>();

        public FoodItemService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<FoodItemResult> UploadFoodItem(FoodItemModel foodItemModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/FoodItems", foodItemModel);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new FoodItemResult { Successful = false, ErrorMessage = errorMessage };
            }

            foodItems.Add(foodItemModel); //Hinzufügen des geuploadeten FoodItems

            return new FoodItemResult { Successful = true };
        }

    }
}