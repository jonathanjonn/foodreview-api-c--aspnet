using FoodReview.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodReview.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Food> Food { get; set; }

        public DbSet<FoodOwner> FoodOwners { get; set; }
        public DbSet<FoodCategory> FoodCategories{ get; set; }
        public DbSet<Review> Reviews{ get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodCategory>()
                .HasKey(fc => new { fc.FoodId, fc.CategoryId });

            modelBuilder.Entity<FoodCategory>()
                .HasOne(f => f.Food)
                .WithMany(fc => fc.FoodCategories)
                .HasForeignKey(c => c.FoodId);

            modelBuilder.Entity<FoodCategory>()
                .HasOne(f => f.Category)
                .WithMany(fc => fc.FoodCategories)
                .HasForeignKey(c => c.CategoryId);


            modelBuilder.Entity<FoodOwner>()
                .HasKey(fo => new { fo.FoodId, fo.OwnerId });

            modelBuilder.Entity<FoodOwner>()
                .HasOne(f => f.Food)
                .WithMany(fc => fc.FoodOwners)
                .HasForeignKey(c => c.FoodId);

            modelBuilder.Entity<FoodOwner>()
                .HasOne(f => f.Owner)
                .WithMany(fc => fc.FoodOwners)
                .HasForeignKey(c => c.OwnerId);
        }

    }
}
