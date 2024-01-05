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
    public class ReviewerController : ControllerBase
    {
        private readonly ReviewerInterfaceRepository ReviewerRepository;
        private readonly IMapper Mapper;

        public ReviewerController(ReviewerInterfaceRepository reviewerRepository, IMapper mapper)
        {
            ReviewerRepository = reviewerRepository;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]

        public IActionResult GetReviewers()
        {
            var reviewers = Mapper.Map<List<ReviewerDto>>(ReviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]

        public IActionResult GetReview(int reviewerId)
        {
            if (!ReviewerRepository.ReviewerExist(reviewerId))
            {
                return NotFound();
            }

            var reviewer = Mapper.Map<ReviewerDto>(ReviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!ReviewerRepository.ReviewerExist(reviewerId))
            {
                return NotFound();
            }

            var reviews = Mapper.Map<List<ReviewDto>>(ReviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto CreateNewReviewer)
        {
            if (CreateNewReviewer == null)
            {
                return BadRequest(ModelState);
            }

            var reviewers = ReviewerRepository.GetReviewers()
                .Where(c => c.LastName.Trim().ToUpper() == CreateNewReviewer.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if (reviewers != null)
            {
                ModelState.AddModelError("", "Revieweer already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewerMap = Mapper.Map<Reviewer>(CreateNewReviewer);

            if (!ReviewerRepository.CreatReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewerId != updatedReviewer.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ReviewerRepository.ReviewerExist(reviewerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var reviewerMap = Mapper.Map<Reviewer>(updatedReviewer);

            if (!ReviewerRepository.UpdateReviewer(reviewerMap))

            {
                ModelState.AddModelError("", "Something went wrong while updating reviewer");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    } 
}
