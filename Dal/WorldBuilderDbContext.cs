using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dal.Entities;

namespace Dal;
public class WorldBuilderDbContext(DbContextOptions<WorldBuilderDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Planet>(planet =>
        {
            planet.HasOne(w => w.Creator)
                .WithMany(u => u.Planets)
                .HasPrincipalKey(u => u.UserName)
                .HasForeignKey(w => w.CreatorUsername);
            
            planet.HasMany(p => p.Calendars)
                .WithOne(c => c.Planet)
                .HasForeignKey(c => c.PlanetId);
            
            planet.HasIndex(w => new { w.CreatorUsername, w.Name })
                .IsUnique();
            
            planet.OwnsOne(w => w.LandColor);
            planet.OwnsOne(w => w.AntiLandColor);
        });

        builder.Entity<User>(user =>
        {
            user.HasIndex(u => u.Email).IsUnique();
            user.HasAlternateKey(u => u.UserName);
            user.HasMany(u => u.Planets).
                WithOne(w => w.Creator);
        });

        builder.Entity<Continent>(continent =>
        {
            continent.HasOne(c => c.Planet)
                .WithMany(w => w.Continents)
                .HasForeignKey(c => c.PlanetId);
            continent.HasOne(c => c.ParentContinent)
                .WithMany(c => c.ChildContinents)
                .HasForeignKey(c => c.ParentContinentId);
            continent.OwnsMany(c => c.Bounds);
            continent.HasMany(c => c.Regions)
                .WithOne(r => r.Continent)
                .HasForeignKey(r => r.ContinentId);
        });

        builder.Entity<Region>(region =>
        {
            region.OwnsOne(r => r.Color);
            region.OwnsMany(r => r.Bounds);
            region.HasMany(r => r.Events)
                .WithOne(e => e.Region)
                .HasForeignKey(e => e.RegionId);
        });
        
        builder.Entity<Calendar>(calendar =>
        {
            calendar.OwnsMany(c => c.YearPhases);
        });
        
        builder.Entity<HistoricalEvent>(hEvent =>
        {
            hEvent
                .HasOne(e => e.DefaultCalendar)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.DefaultCalendarId)
                .OnDelete(DeleteBehavior.NoAction);
            hEvent
                .Ignore(e => e.RelativeStart)
                .Ignore(e => e.RelativeEnd);
        });
        
    }

    public DbSet<Planet> Planets => Set<Planet>();
    public DbSet<Continent> Continents => Set<Continent>();
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<Calendar> Calendars => Set<Calendar>();
    public DbSet<HistoricalEvent> Events => Set<HistoricalEvent>();
}
