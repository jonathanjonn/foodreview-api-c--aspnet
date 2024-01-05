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
    public class ReviewController : ControllerBase
    {
        private readonly ReviewInterfaceRepository ReviewRepository;
        private readonly ReviewerInterfaceRepository ReviewerRepository;
        private readonly FoodInterfaceRepository FoodRepository;
        private readonly IMapper Mapper;

        public ReviewController(ReviewInterfaceRepository reviewRepository, FoodInterfaceRepository foodRepository,
            ReviewerInterfaceRepository reviewerRepository, IMapper mapper)
        {
            ReviewRepository = reviewRepository;
            ReviewerRepository = reviewerRepository;
            FoodRepository = foodRepository;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviews()
        {
            var reviews = Mapper.Map<List<ReviewDto>>(ReviewRepository.GetReviews());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]

        public IActionResult GetReview(int reviewId)
        {
            if (!ReviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var review = Mapper.Map<ReviewDto>(ReviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(review);
        }

        [HttpGet("food/{foodId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewsForFood(int foodId)
        {
            var review = Mapper.Map<List<ReviewDto>>(ReviewRepository.GetReviewByFood(foodId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId,[FromQuery] int foodId, [FromBody] ReviewDto CreateNewReview)
        {
            if (CreateNewReview == null)
            {
                return BadRequest(ModelState);
            }

            var reviews = ReviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == CreateNewReview.Title.TrimEnd().ToUpper()).FirstOrDefault();
            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewMap = Mapper.Map<Review>(CreateNewReview);
            reviewMap.Food = FoodRepository.GetFood(foodId);
            reviewMap.Reviewer = ReviewerRepository.GetReviewer(reviewerId);

            if (!ReviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewId != updatedReview.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ReviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var reviewMap = Mapper.Map<Review>(updatedReview);

            if (!ReviewRepository.UpdateReview(reviewMap))

            {
                ModelState.AddModelError("", "Something went wrong while updating review");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
