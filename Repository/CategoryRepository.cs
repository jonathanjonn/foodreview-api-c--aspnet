using FoodReview.Data;
using FoodReview.Interface;
using FoodReview.Models;

namespace FoodReview.Repository
{
    public class CategoryRepository : CategoryInterfaceRepository
    {
        private DataContext DataContext;

        public CategoryRepository(DataContext dataContext)
        {
            DataContext = dataContext;
        }
        public bool CategoryExist(int id)
        {
            return DataContext.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {

            DataContext.Add(category);
            DataContext.SaveChanges();
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            DataContext.Remove(category); return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return DataContext.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return DataContext.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Food> GetFoodByCategory(int categoryId)
        {
            return DataContext.FoodCategories.Where(fc => fc.CategoryId == categoryId).Select(c => c.Food).ToList();
        }

        public bool Save()
        {
            var saved = DataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            DataContext.Update(category);
            return Save();

        }
    }
}
