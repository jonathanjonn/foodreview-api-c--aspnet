using FoodReview.Data;
using FoodReview.Interface;
using FoodReview.Models;

namespace FoodReview.Repository
{
    public class FoodRepository : FoodInterfaceRepository
    {
        private readonly DataContext dataContext;

        public FoodRepository(DataContext DataContext)
        {
            dataContext = DataContext;
        }

        public ICollection<Food> GetFoods()
        {
            return dataContext.Food.OrderBy(f => f.Id).ToList();
        }

        public bool FoodExists(int foodId)
        {

            return dataContext.Food.Any(f => f.Id == foodId);
        }

        public Food GetFood(int id)
        {
            return dataContext.Food.Where(f => f.Id == id).FirstOrDefault();
        }

        public Food GetFood(string name)
        {
            return dataContext.Food.Where(f => f.Name == name).FirstOrDefault();
        }

        public decimal GetFoodRating(int foodId)
        {
            var review = dataContext.Reviews.Where(f => f.Food.Id == foodId);

            if (review.Count() <= 0)
            {
                return 0;
            }

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateFood(int ownerId, int categoryId, Food food)
        {
            var foodOwnerEntity = dataContext.Owners.Where(fo => fo.Id == ownerId).FirstOrDefault();
            var category = dataContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var foodOwner = new FoodOwner()
            {
                Owner = foodOwnerEntity,
                Food = food,
            };

            dataContext.Add(foodOwner);

            var foodCategory = new FoodCategory()
            {
                Category = category,
                Food = food
            };

            dataContext.Add(foodCategory);

            dataContext.Add(food);

            return Save();
        }

        public bool UpdateFood(int ownerId, int categoryId, Food food)
        {
            dataContext.Update(food); return Save();
        }

        public bool DeleteFood(Food food)
        {
            dataContext.Remove(food); return Save();
        }
    }   
}
