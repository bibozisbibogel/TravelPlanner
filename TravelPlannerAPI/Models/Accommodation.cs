using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models;

public class Accommodation
{
    public Guid Id { get; set; }

    public Guid TripId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Address { get; set; }

    [Required]
    public DateOnly CheckIn { get; set; }

    [Required]
    public DateOnly CheckOut { get; set; }

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal PricePerNight { get; set; }

    [MaxLength(100)]
    public string? BookingReference { get; set; }

    [Required, MaxLength(50)]
    public string AccommodationType { get; set; } = "hotel"; // hotel, hostel, apartment, resort, camping, other

    [Column(TypeName = "decimal(3,2)")]
    public decimal? Rating { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(TripId))]
    public Trip? Trip { get; set; }
}
