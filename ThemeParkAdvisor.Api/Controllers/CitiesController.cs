using Microsoft.AspNetCore.Mvc;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;

        public CitiesController(
            ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        /// <summary>
        /// Retrieves a list of all cities with their IDs.
        /// </summary>
        [HttpPost("cities")]
        public async Task<ActionResult<IEnumerable<CityNameDto>>> GetCitiesFiltered([FromBody] CityFilterDto filterDto)
        {
            var filter = filterDto.ToDomain();
            var cities = await _cityRepository.GetCitiesAsync(filter);
            var result = cities.Select(p => p.ToDto());

            return Ok(result);
        }
    }
}
