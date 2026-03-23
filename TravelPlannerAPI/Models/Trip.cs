using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models;

public class Trip
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid DestinationId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    public int TravelersCount { get; set; } = 1;

    [Column(TypeName = "decimal(12,2)")]
    public decimal? TotalBudget { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "draft"; // draft, planning, confirmed, completed, cancelled

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(DestinationId))]
    public Destination Destination { get; set; } = null!;

    public ICollection<ItineraryDay> ItineraryDays { get; set; } = new List<ItineraryDay>();
    public ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<TripCollaborator> Collaborators { get; set; } = new List<TripCollaborator>();
}
