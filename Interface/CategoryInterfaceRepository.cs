using FoodReview.Models;

namespace FoodReview.Interface
{
    public interface CategoryInterfaceRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);

        ICollection<Food> GetFoodByCategory(int categoryId);

        bool CategoryExist(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
