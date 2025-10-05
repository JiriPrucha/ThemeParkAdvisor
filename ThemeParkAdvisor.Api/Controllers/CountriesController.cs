using Microsoft.AspNetCore.Mvc;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;

        public CountriesController(
            ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        /// <summary>
        /// Retrieves a list of all countries with their IDs and names.
        /// </summary>
        [HttpPost("countries")]
        public async Task<ActionResult<IEnumerable<CountryNameDto>>> GetCitiesFiltered([FromBody] CountryFilterDto filterDto)
        {
            var filter = filterDto.ToDomain();
            var cities = await _countryRepository.GetCountriesAsync(filter);
            var result = cities.Select(p => p.ToDto());

            return Ok(result);
        }
    }
}
