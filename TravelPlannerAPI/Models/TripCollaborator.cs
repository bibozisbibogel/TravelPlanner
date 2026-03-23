using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models;

public class TripCollaborator
{
    public Guid Id { get; set; }

    public Guid TripId { get; set; }

    public Guid UserId { get; set; }

    [Required, MaxLength(50)]
    public string Role { get; set; } = "viewer"; // editor, viewer

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(TripId))]
    public Trip Trip { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
