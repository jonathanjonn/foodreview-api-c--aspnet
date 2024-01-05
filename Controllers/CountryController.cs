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
    public class CountryController : ControllerBase
    {
        private readonly CountryInterfaceRepository CountryRepository;
        private readonly IMapper Mapper;

        public CountryController(CountryInterfaceRepository countryRepository, IMapper mapper)
        {
            CountryRepository = countryRepository;
            Mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]

        public IActionResult GetCountries()
        {
            var countries = Mapper.Map<List<CountryDto>>(CountryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]

        public IActionResult GetCountry(int countryId)
        {
            if (!CountryRepository.CountryExist(countryId))
            {
                return NotFound();
            }

            var country = Mapper.Map<CountryDto>(CountryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]

        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = Mapper.Map<CountryDto>(CountryRepository.GetCountry(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto CreateNewCountry)
        {
            if (CreateNewCountry== null)
            {
                return BadRequest(ModelState);
            }

            var country = CountryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == CreateNewCountry.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Country already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryMap = Mapper.Map<Country>(CreateNewCountry);
            if (!CountryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
            {
                return BadRequest(ModelState);
            }

            if (countryId != updatedCountry.Id)
            {
                return BadRequest(ModelState);
            }

            if (!CountryRepository.CountryExist(countryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var countryMap = Mapper.Map<Country>(updatedCountry);

            if (!CountryRepository.UpdateCountry(countryMap))

            {
                ModelState.AddModelError("", "Something went wrong while updating country");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteCountry(int countryId)
        {
            if (!CountryRepository.CountryExist(countryId))
            {
                return NotFound();
            }

            var delCountry = CountryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!CountryRepository.DeleteCountry(delCountry))
            {
                ModelState.AddModelError("", "Something went wrong while deleting country");
            }
            return NoContent();
        }
    }
}
