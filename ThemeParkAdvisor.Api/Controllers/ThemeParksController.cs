using Microsoft.AspNetCore.Mvc;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThemeParksController : ControllerBase
    {
        private readonly IThemeParkRepository _parkRepository;
        private readonly IThemeParkRecommendationService _recommendationService;

        public ThemeParksController(
            IThemeParkRepository parkRepository,
            IThemeParkRecommendationService recommendationService)
        {
            _parkRepository = parkRepository;
            _recommendationService = recommendationService;
        }

        /// <summary>
        /// Retrieves a list of all theme park names with their IDs.
        /// </summary>
        [HttpPost("names")]
        public async Task<ActionResult<IEnumerable<ThemeParkNameDto>>> GetNames([FromBody] ThemeParkNameFilterDto filterDto)
        {
            var filter = filterDto.ToDomain();
            var parks = await _parkRepository.GetThemeParkNamesAsync(filter);
            var result = parks.Select(p => p.ToDto());

            return Ok(result);
        }

        /// <summary>
        /// Searches for theme parks based on provided filter criteria.
        /// </summary>
        /// <param name="filterDto">Filter object containing search parameters.</param>
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<ThemeParkDto>>> Search([FromBody] ThemeParkFilterDto filterDto)
        {
            var filter = filterDto.ToDomain();
            var parks = await _parkRepository.GetThemeParksAsync(filter);
            var result = parks.Select(p => p.ToDto());

            return Ok(result);
        }

        /// <summary>
        /// Generates theme park recommendations based on user preferences.
        /// </summary>
        /// <param name="preferencesDto">User preferences for theme park recommendations.</param>
        [HttpPost("recommendations")]
        public async Task<ActionResult<IEnumerable<ThemeParkRecommendationDto>>> Recommend([FromBody] ThemeParkPreferencesDto preferencesDto)
        {
            var preferences = preferencesDto.ToDomain();
            var recommendations = await _recommendationService.RecommendAsync(preferences);
            var result = recommendations.Select(p => p.ToDto());

            return Ok(result);
        }
    }
}