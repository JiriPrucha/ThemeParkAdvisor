using Microsoft.AspNetCore.Mvc;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Shared;

namespace ThemeParkAdvisor.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttractionsController : ControllerBase
    {
        private readonly IAttractionRepository _attractionRepository;
        private readonly IAttractionRecommendationService _recommendationService;

        public AttractionsController(
            IAttractionRepository repository,
            IAttractionRecommendationService recommendationService)
        {
            _attractionRepository = repository;
            _recommendationService = recommendationService;
        }

        /// <summary>
        /// Retrieves all attractions for a specific theme park.
        /// </summary>
        /// <param name="themeParkId">The ID of the theme park.</param>
        [HttpGet("{themeParkId}")]
        public async Task<ActionResult<IEnumerable<AttractionDto>>> GetByThemeParkId(int themeParkId)
        {
            var attractions = await _attractionRepository.GetAttractionsByThemeParkIdAsync(themeParkId);
            var result = attractions.Select(a => a.ToDto());

            return Ok(result);
        }

        /// <summary>
        /// Searches for attractions based on provided filter criteria.
        /// </summary>
        /// <param name="filterDto">Filter object containing search parameters.</param>
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<AttractionDto>>> Search([FromBody] AttractionFilterDto filterDto)
        {
            var filter = filterDto.ToDomain();
            var attractions = await _attractionRepository.GetAttractionsAsync(filter);
            var result = attractions.Select(a => a.ToDto());

            return Ok(result);
        }

        /// <summary>
        /// Generates attraction recommendations based on user preferences.
        /// </summary>
        /// <param name="preferencesDto">User preferences for attraction recommendations.</param>
        [HttpPost("recommendations")]
        public async Task<ActionResult<IEnumerable<AttractionRecommendationDto>>> Recommend([FromBody] AttractionPreferencesDto preferencesDto)
        {
            var preferences = preferencesDto.ToDomain();
            var attractions = await _recommendationService.RecommendAsync(preferences);
            var result = attractions.Select(a => a.ToDto());

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all attraction types from the database
        /// </summary>
        [HttpGet("attractionTypes")]
        public async Task<ActionResult<IEnumerable<AttractionTypeDto>>> GetAttractionTypes()
        {
            var attractionTypes = await _attractionRepository.GetAttractionTypes();
            var result = attractionTypes.Select(at => at.ToDto());

            return Ok(result);
        }
    }
}