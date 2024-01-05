using FoodReview.Models;

namespace FoodReview.Interface
{
    public interface FoodInterfaceRepository
    {
        ICollection<Food> GetFoods();
        Food GetFood(int id);
        Food GetFood(string name);
        decimal GetFoodRating(int foodId);
        bool FoodExists(int foodId);

        bool CreateFood(int ownerId, int categoryId, Food food);
        bool UpdateFood(int ownerId, int categoryId, Food food);
        bool DeleteFood(Food food);
        bool Save();
    }
}
