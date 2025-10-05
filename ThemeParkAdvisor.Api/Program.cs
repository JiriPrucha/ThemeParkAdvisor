using Microsoft.EntityFrameworkCore;
using ThemeParkAdvisor.Application;
using ThemeParkAdvisor.Domain;
using ThemeParkAdvisor.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON configuration
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context configuration
builder.Services.AddDbContext<ThemeParkDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository registrations
builder.Services.AddScoped<IThemeParkRepository, ThemeParkRepository>();
builder.Services.AddScoped<IAttractionRepository, AttractionRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

// Recommendation service interfaces and implementations
builder.Services.AddScoped<IThemeParkRecommendationService, ThemeParkRecommendationService>();
builder.Services.AddScoped<IAttractionRecommendationService, AttractionRecommendationService>();

// Specific strategies for theme parks
builder.Services.AddScoped<IThemeParkScoringStrategy, LocationThemeParkScoringStrategy>();
builder.Services.AddScoped<IThemeParkScoringStrategy, AdrenalineMatchStrategy>();
builder.Services.AddScoped<IThemeParkScoringStrategy, SimilarThemeParkScoringStrategy>();
builder.Services.AddScoped<IThemeParkScoringStrategy, AgeBasedThemeParkScoringStrategy>();

// Fallback strategies for theme parks
builder.Services.AddScoped<IFallbackThemeParkScoringStrategy, StatisticsBasedThemeParkSelectionStrategy>();
builder.Services.AddScoped<IFallbackThemeParkScoringStrategy, RandomThemeParkScoringStrategy>();

// Fallback strategies for attractions
builder.Services.AddScoped<IFallbackAttractionSelectionStrategy, StatisticsBasedAttractionSelectionStrategy>();

// Aggregators
builder.Services.AddScoped<ThemeParkScoringAggregator>();
builder.Services.AddScoped<AttractionScoringAggregator>();

// Mapping generic aggregator types to their concrete implementations
builder.Services.AddScoped<GenericScoringAggregator<ThemePark, ThemeParkPreferences>, ThemeParkScoringAggregator>();
builder.Services.AddScoped<GenericScoringAggregator<Attraction, AttractionPreferences>, AttractionScoringAggregator>();

// Recommendation services (concrete implementations)
builder.Services.AddScoped<ThemeParkRecommendationService>();
builder.Services.AddScoped<AttractionRecommendationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
