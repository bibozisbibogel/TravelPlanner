using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models;

public class ItineraryActivity
{
    public Guid Id { get; set; }

    public Guid ItineraryDayId { get; set; }

    public TimeOnly? Time { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [MaxLength(300)]
    public string? Location { get; set; }

    [Required, MaxLength(50)]
    public string Category { get; set; } = "sightseeing"; // sightseeing, food, transport, shopping, entertainment, nature, other

    [Column(TypeName = "decimal(10,2)")]
    public decimal EstimatedCost { get; set; } = 0;

    public int SortOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ItineraryDayId))]
    public ItineraryDay ItineraryDay { get; set; } = null!;
}
