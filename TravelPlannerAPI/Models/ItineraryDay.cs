using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models;

public class ItineraryDay
{
    public Guid Id { get; set; }

    public Guid TripId { get; set; }

    [Required]
    public int DayNumber { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(TripId))]
    public Trip? Trip { get; set; }

    public ICollection<ItineraryActivity> Activities { get; set; } = new List<ItineraryActivity>();
}
