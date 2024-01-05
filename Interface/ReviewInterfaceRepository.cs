using FoodReview.Models;

namespace FoodReview.Interface
{
    public interface ReviewInterfaceRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewByFood(int foodId);

        bool ReviewExists(int reviewId);

        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool Save();
        bool DeleteReviews(List<Review> reviews);
    }
}
