using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
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

            var uploadResult = await response.Content.ReadFromJsonAsync<FoodItemResult>();
            return uploadResult ?? new FoodItemResult { Successful = false, ErrorMessage = "Error occurred during food item upload" };
        }
    }
}