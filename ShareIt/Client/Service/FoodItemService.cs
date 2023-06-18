using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ShareIt.Shared;
using System.Net.Http.Json;

namespace ShareIt.Client.Service
{
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

            foodItems.Add(foodItemModel); // Add the uploaded food item to the list

            return new FoodItemResult { Successful = true };
        }

        public async Task<FoodItemResult> UpdateFoodItem(int id, FoodItemModel foodItemModel)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/FoodItems/{id}", foodItemModel);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new FoodItemResult { Successful = false, ErrorMessage = errorMessage };
            }

            // Handle the response accordingly, e.g., update the local foodItems list

            return new FoodItemResult { Successful = true };
        }

    }
}