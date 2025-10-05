using Microsoft.EntityFrameworkCore;
using ThemeParkAdvisor.Domain;

namespace ThemeParkAdvisor.Infrastructure
{
    public class ThemeParkDbContext : DbContext
    {
        public ThemeParkDbContext(DbContextOptions<ThemeParkDbContext> options)
            : base(options) { }

        public DbSet<ThemePark> ThemeParks => Set<ThemePark>();

        public DbSet<Attraction> Attractions => Set<Attraction>();

        public DbSet<City> Cities => Set<City>();

        public DbSet<Region> Regions => Set<Region>();
        
        public DbSet<Country> Countries => Set<Country>();

        public DbSet<AttractionType> AttractionTypes => Set<AttractionType>();

    }
}
