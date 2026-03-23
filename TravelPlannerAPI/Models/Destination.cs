using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models;

public class Destination
{
    public Guid Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Continent { get; set; } = string.Empty; // europe, asia, north_america, south_america, africa, oceania

    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required, MaxLength(50)]
    public string Category { get; set; } = "city_break"; // city_break, beach, mountain, cultural, adventure, wellness

    [Column(TypeName = "decimal(10,2)")]
    public decimal? AvgBudgetPerDay { get; set; }

    [Column(TypeName = "decimal(3,2)")]
    public decimal? Rating { get; set; }

    [Column(TypeName = "decimal(10,7)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(10,7)")]
    public decimal? Longitude { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
