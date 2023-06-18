using ShareIt.Shared;

namespace ShareIt.Client.Service
{
    public interface IFoodItemService
    {
        Task<FoodItemResult> UploadFoodItem(FoodItemModel foodItemModel);
        Task<FoodItemResult> UpdateFoodItem(int foodItemId, FoodItemModel updatedFoodItem);
    }
}
