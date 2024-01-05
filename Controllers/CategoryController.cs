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
    public class CategoryController : ControllerBase
    {
        private readonly CategoryInterfaceRepository CategoryRepository;
        private readonly IMapper Mapper;

        public CategoryController(CategoryInterfaceRepository categoryRepository, IMapper mapper)
        {
            CategoryRepository = categoryRepository;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]

        public IActionResult GetCategories()
        {
            var categories = Mapper.Map<List<CategoryDto>>(CategoryRepository.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]

        public IActionResult GetCategory(int categoryId)
        {
            if (!CategoryRepository.CategoryExist(categoryId))
            {
                return NotFound();
            }

            var category = Mapper.Map<CategoryDto>(CategoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);
        }

        [HttpGet("food/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]

        public IActionResult GetFoodCategoryId(int categoryId)
        {
            var foods = Mapper.Map<List<FoodDto>>(
                CategoryRepository.GetFoodByCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(foods);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto CreateNewCateogry)
        {
            if (CreateNewCateogry == null)
            {
                return BadRequest(ModelState);
            }

            var category = CategoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == CreateNewCateogry.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "Category already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryMap = Mapper.Map<Category>(CreateNewCateogry);
            if (!CategoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
            {
                return BadRequest(ModelState);
            }

            if (categoryId != updatedCategory.Id)
            {
                return BadRequest(ModelState);
            }

            if (!CategoryRepository.CategoryExist(categoryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var categoryMap = Mapper.Map<Category>(updatedCategory);

            if (!CategoryRepository.UpdateCategory(categoryMap))

            {
                ModelState.AddModelError("", "Something went wrong while updating category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteCategory(int categoryId) 
        {
            if (!CategoryRepository.CategoryExist(categoryId))
            {
                return NotFound();
            }

            var delCategory = CategoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!CategoryRepository.DeleteCategory(delCategory))
            {
                ModelState.AddModelError("", "Something went wrong while deleting category");
            }
            return NoContent();
        }
    }
}
