using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models;

public class Expense
{
    public Guid Id { get; set; }

    public Guid TripId { get; set; }

    [Required, MaxLength(50)]
    public string Category { get; set; } = string.Empty; // accommodation, food, transport, activities, shopping, other

    [Required, Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required, MaxLength(3)]
    public string Currency { get; set; } = "EUR";

    [Required, MaxLength(300)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateOnly Date { get; set; }

    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "card"; // card, cash, transfer

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(TripId))]
    public Trip Trip { get; set; } = null!;
}
