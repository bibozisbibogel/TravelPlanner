using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Destinations.AnyAsync())
            return;

        var destinations = new List<Destination>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Tokyo",
                Country = "Japan",
                Continent = "asia",
                Category = "cultural",
                Description = "The city of the future with ancient temples, refined gastronomy and a fascinating culture.",
                AvgBudgetPerDay = 150,
                Rating = 4.9m,
                ImageUrl = "https://images.unsplash.com/photo-1540959733332-eab4deabeeaf?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Paris",
                Country = "France",
                Continent = "europe",
                Category = "city_break",
                Description = "The City of Light with its iconic architecture, world-renowned museums and refined gastronomy.",
                AvgBudgetPerDay = 180,
                Rating = 4.8m,
                ImageUrl = "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Bali",
                Country = "Indonesia",
                Continent = "asia",
                Category = "beach",
                Description = "The island of the gods with tropical beaches, rice terraces and a unique spirituality.",
                AvgBudgetPerDay = 60,
                Rating = 4.7m,
                ImageUrl = "https://images.unsplash.com/photo-1537996194471-e657df975ab4?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "New York",
                Country = "USA",
                Continent = "north_america",
                Category = "city_break",
                Description = "The city that never sleeps, with impressive skyscrapers and a unique energy.",
                AvgBudgetPerDay = 300,
                Rating = 4.6m,
                ImageUrl = "https://images.unsplash.com/photo-1496442226666-8d4d0e62e6e9?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Barcelona",
                Country = "Spain",
                Continent = "europe",
                Category = "beach",
                Description = "Gaudi architecture, Mediterranean beaches and a legendary nightlife.",
                AvgBudgetPerDay = 130,
                Rating = 4.7m,
                ImageUrl = "https://images.unsplash.com/photo-1539037116277-4db20889f2d4?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Kenya Safari",
                Country = "Kenya",
                Continent = "africa",
                Category = "adventure",
                Description = "Big Five, endless savannas and unforgettable sunsets in the heart of Africa.",
                AvgBudgetPerDay = 350,
                Rating = 4.9m,
                ImageUrl = "https://images.unsplash.com/photo-1516026672322-bc52d61a55d5?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Santorini",
                Country = "Greece",
                Continent = "europe",
                Category = "beach",
                Description = "White houses with blue domes, romantic sunsets and excellent local wines.",
                AvgBudgetPerDay = 170,
                Rating = 4.8m,
                ImageUrl = "https://images.unsplash.com/photo-1570077188670-e3a8d69ac5ff?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Machu Picchu",
                Country = "Peru",
                Continent = "south_america",
                Category = "adventure",
                Description = "The Inca citadel in the Andes Mountains \u2013 one of the seven wonders of the world.",
                AvgBudgetPerDay = 120,
                Rating = 4.9m,
                ImageUrl = "https://images.unsplash.com/photo-1587595431973-160d0d94add1?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Amsterdam",
                Country = "Netherlands",
                Continent = "europe",
                Category = "city_break",
                Description = "Picturesque canals, renowned museums, bicycles and legendary tolerance.",
                AvgBudgetPerDay = 140,
                Rating = 4.6m,
                ImageUrl = "https://images.unsplash.com/photo-1534351590666-13e3e96b5017?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Norway",
                Country = "Norway",
                Continent = "europe",
                Category = "mountain",
                Description = "Impressive fjords, northern lights and arctic landscapes of rare beauty.",
                AvgBudgetPerDay = 280,
                Rating = 4.8m,
                ImageUrl = "https://images.unsplash.com/photo-1531366936337-7c912a4589a7?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Rome",
                Country = "Italy",
                Continent = "europe",
                Category = "cultural",
                Description = "The Eternal City with the Colosseum, Vatican, Trevi Fountain and authentic pasta.",
                AvgBudgetPerDay = 130,
                Rating = 4.7m,
                ImageUrl = "https://images.unsplash.com/photo-1552832230-c0197dd311b5?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Maldives",
                Country = "Maldives",
                Continent = "asia",
                Category = "beach",
                Description = "Overwater bungalows, coral reefs and crystal-clear turquoise water.",
                AvgBudgetPerDay = 400,
                Rating = 4.9m,
                ImageUrl = "https://images.unsplash.com/photo-1514282401047-d79a71a590e8?w=400&q=80",
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Destinations.AddRange(destinations);
        await context.SaveChangesAsync();
    }
}
