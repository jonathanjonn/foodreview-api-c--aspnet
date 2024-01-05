using FoodReview.Data;
using FoodReview.Interface;
using FoodReview.Models;

namespace FoodReview.Repository
{
    public class OwnerRepository : OwnerInterfaceRepository
    {
        private readonly DataContext DataContext;

        public OwnerRepository(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public bool CreateOwner(Owner owner)
        {
            DataContext.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            DataContext.Remove(owner);return Save();
        }

        public ICollection<Food> GetFoodByOwner(int ownerId)
        {
            return DataContext.FoodOwners.Where(f => f.Owner.Id == ownerId).Select(f => f.Food).ToList();
        }

        public Owner GetOwner(int ownerId)
        {
            return DataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerByFood(int foodId)
        {
            return DataContext.FoodOwners.Where(f => f.Food.Id == foodId).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return DataContext.Owners.ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return DataContext.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = DataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            DataContext.Update(owner); return Save();
        }
    }
}
