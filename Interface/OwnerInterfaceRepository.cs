using FoodReview.Models;

namespace FoodReview.Interface
{
    public interface OwnerInterfaceRepository
    {
        ICollection<Owner> GetOwners();

        Owner GetOwner(int ownerId);

        ICollection<Owner> GetOwnerByFood(int foodId);

        ICollection<Food> GetFoodByOwner(int ownerId);

        bool OwnerExists(int ownerId);

        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}
