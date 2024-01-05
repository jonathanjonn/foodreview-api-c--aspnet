using FoodReview.Data;
using FoodReview.Models;

namespace FoodReview
{
    public class Seed
    {

        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.FoodOwners.Any())
            {
                var foodOwners = new List<FoodOwner>()
                {
                    new FoodOwner()
                    {
                        Food = new Food()
                        {
                            Name = "Rendang",
                            Description = "daging sapi",
                            FoodCategories = new List<FoodCategory>()
                            {
                                new FoodCategory { Category = new Category() { Name = "Indonesian"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Rendang",Text = "Rendang makanan padang", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "user1", LastName = "pertama" } },
                                new Review { Title="Rendang", Text = "Rendang enak", Rating = 4,
                                Reviewer = new Reviewer(){ FirstName = "user2", LastName = "kedua" } },
                                new Review { Title="Rendang",Text = "Tidak suka rendang", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "user3", LastName = "ketiga" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Nasi",
                            LastName = "Padang",
                            Country = new Country()
                            {
                                Name = "Padang"
                            }
                        }
                    },
                    new FoodOwner()
                    {
                        Food = new Food()
                        {
                            Name = "Pasta",
                            Description = "Pasta, seperti mie yang menggunakan bumbu itali",
                            FoodCategories = new List<FoodCategory>()
                            {
                                new FoodCategory { Category = new Category() { Name = "Italian"}}
                            },
                            Reviews = new List<Review>()
                            {
                               new Review { Title="Pasta",Text = "Pasta makanan itali", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "user1", LastName = "pertama" } },
                                new Review { Title="Pasta", Text = "Pasta enak", Rating = 4,
                                Reviewer = new Reviewer(){ FirstName = "user2", LastName = "kedua" } },
                                new Review { Title="Pasta",Text = "Tidak suka Pasta", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "user3", LastName = "ketiga" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Chef",
                            LastName = "Itali",
                            Country = new Country()
                            {
                                Name = "Paris"
                            }
                        }
                    },
                        new FoodOwner()
                    {
                        Food = new Food()
                        {
                            Name = "Nasi Goreng",
                            Description = "Nasi di goreng menggunakan bumbu",
                            FoodCategories = new List<FoodCategory>()
                            {
                                new FoodCategory { Category = new Category() { Name = "Asian"}}
                            },
                            Reviews = new List<Review>()
                            {
                               new Review { Title="Nasi Goreng",Text = "Nasi Goreng makanan asia", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "user1", LastName = "pertama" } },
                                new Review { Title="Nasi Goreng", Text = "Nasi Goreng enak", Rating = 4,
                                Reviewer = new Reviewer(){ FirstName = "user2", LastName = "kedua" } },
                                new Review { Title="Nasi Goreng",Text = "Tidak suka Nasi Goreng", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "user3", LastName = "ketiga" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Uncle",
                            LastName = "Roger",
                            Country = new Country()
                            {
                                Name = "Singapore"
                            }
                        }
                    }
                };
                dataContext.FoodOwners.AddRange(foodOwners);
                dataContext.SaveChanges();
            }
        }
    }
}
