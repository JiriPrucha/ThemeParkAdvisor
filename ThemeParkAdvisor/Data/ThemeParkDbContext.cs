using Microsoft.EntityFrameworkCore;

namespace ThemeParkAdvisor.Data
{
    public class ThemeParkDbContext : DbContext
    {
        public ThemeParkDbContext(DbContextOptions<ThemeParkDbContext> options)
            : base(options) { }

        public DbSet<ThemePark> ThemeParks { get; set; }
    }
}
