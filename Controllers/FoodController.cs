using AutoMapper;
using FoodReview.Dto;
using FoodReview.Interface;
using FoodReview.Models;
using FoodReview.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FoodReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodInterfaceRepository FoodRepository;
        private readonly OwnerInterfaceRepository OwnerRepository;
        private readonly ReviewInterfaceRepository ReviewRepository;
        private readonly IMapper Mapper;

        public FoodController(FoodInterfaceRepository foodRepository, OwnerInterfaceRepository ownerRepository, ReviewInterfaceRepository reviewRepository, IMapper mapper)
        {
            FoodRepository = foodRepository;
            OwnerRepository = ownerRepository;
            ReviewRepository = reviewRepository;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Food>))]

        public IActionResult GetFoods()
        {
            var foods = Mapper.Map<List<FoodDto>>(FoodRepository.GetFoods());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(foods);
        }

        [HttpGet("{foodId}")]
        [ProducesResponseType(200, Type = typeof(Food))]
        [ProducesResponseType(400)]

        public IActionResult GetFood(int foodId)
        {
            if (!FoodRepository.FoodExists(foodId))
            {
                return NotFound();
            }

            var food = Mapper.Map<FoodDto>(FoodRepository.GetFood(foodId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(food);
        }

        [HttpGet("{foodId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]

        public IActionResult GetFoodRating(int foodId)
        {
            if (!FoodRepository.FoodExists(foodId))
            {
                return NotFound();
            }

            var rating = FoodRepository.GetFoodRating(foodId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateFood([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] FoodDto CreateNewFood)
        {
            if (CreateNewFood == null)
            {
                return BadRequest(ModelState);
            }

            var foods = FoodRepository.GetFoods()
                .Where(c => c.Name.Trim().ToUpper() == CreateNewFood.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (foods != null)
            {
                ModelState.AddModelError("", "Food already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var foodMap = Mapper.Map<Food>(CreateNewFood);

            if (!FoodRepository.CreateFood(ownerId, categoryId, foodMap))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{foodId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFood([FromQuery] int categoryId, int foodId, [FromQuery] int ownerId,[FromBody] FoodDto updatedFood)
        {
            if (updatedFood == null)
            {
                return BadRequest(ModelState);
            }

            if (foodId != updatedFood.Id)
            {
                return BadRequest(ModelState);
            }

            if (!FoodRepository.FoodExists(foodId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var foodMap = Mapper.Map<Food>(updatedFood);

            if (!FoodRepository.UpdateFood(ownerId,categoryId,foodMap))

            {
                ModelState.AddModelError("", "Something went wrong while updating food");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{foodId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteFood(int foodId)
        {
            if (!FoodRepository.FoodExists(foodId))
            {
                return NotFound();
            }

            var delReviews = ReviewRepository.GetReviewByFood(foodId);
            var delFood = FoodRepository.GetFood(foodId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!ReviewRepository.DeleteReviews(delReviews.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong while deleting reviews");
            }

            if (!FoodRepository.DeleteFood(delFood))
            {
                ModelState.AddModelError("", "Something went wrong while deleting food");
            }
            return NoContent();
        }
    }
}
