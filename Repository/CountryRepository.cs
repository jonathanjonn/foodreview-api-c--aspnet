using AutoMapper;
using FoodReview.Data;
using FoodReview.Interface;
using FoodReview.Models;

namespace FoodReview.Repository
{
    public class CountryRepository : CountryInterfaceRepository
    {
        private readonly DataContext DataContext;
        private readonly IMapper Mapper;

        public CountryRepository(DataContext dataContext, IMapper mapper)
        {
            DataContext = dataContext;
            Mapper = mapper;
        }
        public bool CountryExist(int id)
        {
            return DataContext.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            DataContext.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            DataContext.Remove(country); return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return DataContext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return DataContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return DataContext.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return DataContext.Owners.Where(c => c.Country.Id == countryId).ToList();
        }

        public bool Save()
        {
            var saved = DataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            DataContext.Update(country); return Save();
        }
    }
}
