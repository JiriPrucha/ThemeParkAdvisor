using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThemeParkAdvisor.Data;

namespace ThemeParkAdvisor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeParksController : ControllerBase
    {
        private readonly ThemeParkDbContext _context;

        public ThemeParksController(ThemeParkDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThemePark>>> GetThemeParks()
        {
            return await _context.ThemeParks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ThemePark>> GetThemePark(int id)
        {
            var park = await _context.ThemeParks.FindAsync(id);
            if (park == null) return NotFound();
            return park;
        }

        [HttpGet("testdb")]
        public IActionResult TestDb()
        {
            return Ok(_context.Database.GetDbConnection().ConnectionString);
        }
    }
}
