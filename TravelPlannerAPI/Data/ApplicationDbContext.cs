using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<ItineraryDay> ItineraryDays => Set<ItineraryDay>();
    public DbSet<ItineraryActivity> ItineraryActivities => Set<ItineraryActivity>();
    public DbSet<Accommodation> Accommodations => Set<Accommodation>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<TripCollaborator> TripCollaborators => Set<TripCollaborator>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== User =====
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // ===== Destination =====
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasIndex(d => new { d.Name, d.Country }).IsUnique();
        });

        // ===== Trip =====
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasOne(t => t.User)
                  .WithMany(u => u.Trips)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Destination)
                  .WithMany(d => d.Trips)
                  .HasForeignKey(t => t.DestinationId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== ItineraryDay =====
        modelBuilder.Entity<ItineraryDay>(entity =>
        {
            entity.HasIndex(id => new { id.TripId, id.DayNumber }).IsUnique();

            entity.HasOne(id => id.Trip)
                  .WithMany(t => t.ItineraryDays)
                  .HasForeignKey(id => id.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ===== ItineraryActivity =====
        modelBuilder.Entity<ItineraryActivity>(entity =>
        {
            entity.HasOne(ia => ia.ItineraryDay)
                  .WithMany(id => id.Activities)
                  .HasForeignKey(ia => ia.ItineraryDayId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ===== Accommodation =====
        modelBuilder.Entity<Accommodation>(entity =>
        {
            entity.HasOne(a => a.Trip)
                  .WithMany(t => t.Accommodations)
                  .HasForeignKey(a => a.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ===== Expense =====
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasOne(e => e.Trip)
                  .WithMany(t => t.Expenses)
                  .HasForeignKey(e => e.TripId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ===== TripCollaborator (N:M junction table) =====
        modelBuilder.Entity<TripCollaborator>(entity =>
        {
            entity.HasIndex(tc => new { tc.TripId, tc.UserId }).IsUnique();

            entity.HasOne(tc => tc.Trip)
                  .WithMany(t => t.Collaborators)
                  .HasForeignKey(tc => tc.TripId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tc => tc.User)
                  .WithMany(u => u.TripCollaborations)
                  .HasForeignKey(tc => tc.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
