using FoodReview.Data;
using FoodReview.Interface;
using FoodReview.Models;

namespace FoodReview.Repository
{
    public class ReviewRepository : ReviewInterfaceRepository
    {
        private readonly DataContext DataContext;

        public ReviewRepository(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public bool CreateReview(Review review)
        {
            DataContext.Add(review);
            return Save();    
        }

        public bool DeleteReview(Review review)
        {
            DataContext.Remove(review); return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            DataContext.RemoveRange(reviews); return Save();
        }

        public Review GetReview(int reviewId)
        {
            return DataContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewByFood(int foodId)
        {
            return DataContext.Reviews.Where(r => r.Food.Id == foodId).ToList();
        }

        public ICollection<Review> GetReviews()
        {
            return DataContext.Reviews.ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return DataContext.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            var saved = DataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            DataContext.Update(review); return Save();
        }
    }
}
