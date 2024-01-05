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
    public class OwnerController : ControllerBase
    {
        private readonly OwnerInterfaceRepository OwnerRepository;
        private readonly CountryInterfaceRepository CountryRepository;
        private readonly IMapper Mapper;

        public OwnerController(OwnerInterfaceRepository ownerRepository, CountryInterfaceRepository countryRepository, IMapper mapper)
        {
            OwnerRepository = ownerRepository;
            CountryRepository = countryRepository;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

        public IActionResult GetOwners()
        {
            var owners = Mapper.Map<List<OwnerDto>>(OwnerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public IActionResult GetOwner(int ownerId)
        {
            if (!OwnerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var owner = Mapper.Map<OwnerDto>(OwnerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpGet("{ownerId}/food")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public IActionResult GetFoodByOwner(int ownerId)
        {
            if (!OwnerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var owner = Mapper.Map<List<FoodDto>>(OwnerRepository.GetFoodByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto CreateNewOwner)
        {
            if (CreateNewOwner == null)
            {
                return BadRequest(ModelState);
            }

            var owners = OwnerRepository.GetOwners()
                .Where(c => c.LastName.Trim().ToUpper() == CreateNewOwner.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if (owners != null)
            {
                ModelState.AddModelError("", "Owner already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ownerMap = Mapper.Map<Owner>(CreateNewOwner);

            ownerMap.Country = CountryRepository.GetCountry(countryId);

            if (!OwnerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
            {
                return BadRequest(ModelState);
            }

            if (ownerId != updatedOwner.Id)
            {
                return BadRequest(ModelState);
            }

            if (!OwnerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var ownerMap = Mapper.Map<Owner>(updatedOwner);

            if (!OwnerRepository.UpdateOwner(ownerMap))

            {
                ModelState.AddModelError("", "Something went wrong while updating owner");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteOwner(int ownerId)
        {
            if (!OwnerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var delOwner = OwnerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!OwnerRepository.DeleteOwner(delOwner))
            {
                ModelState.AddModelError("", "Something went wrong while deleting owner");
            }
            return NoContent();
        }
    }
}
