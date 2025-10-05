using Microsoft.AspNetCore.Mvc;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;

        public RegionsController(
            IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        /// <summary>
        /// Retrieves a list of all regions with their IDs and names.
        /// </summary>
        [HttpPost("regions")]
        public async Task<ActionResult<IEnumerable<RegionNameDto>>> GetCitiesFiltered([FromBody] RegionFilterDto filterDto)
        {
            var filter = filterDto.ToDomain();
            var regions = await _regionRepository.GetRegionsAsync(filter);
            var result = regions.Select(p => p.ToDto());

            return Ok(result);
        }
    }
}
