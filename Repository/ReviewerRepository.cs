using FoodReview.Data;
using FoodReview.Interface;
using FoodReview.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodReview.Repository
{
    public class ReviewerRepository : ReviewerInterfaceRepository
    {
        private readonly DataContext DataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public bool CreatReviewer(Reviewer reviewer)
        {
            DataContext.Add(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return DataContext.Reviewers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return DataContext.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return DataContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }
        public bool ReviewerExist(int reviewerId)
        {
            return DataContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Save()
        {
            var saved = DataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            DataContext.Update(reviewer); return Save();
        }
    }
}
