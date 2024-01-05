using AutoMapper;
using FoodReview.Dto;
using FoodReview.Models;

namespace FoodReview.Helper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Food, FoodDto>();
            CreateMap<FoodDto, Food>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();
        }
    }
}
