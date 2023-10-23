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
        builder.Entity<Planet>(world =>
        {
            world.HasOne(w => w.Creator)
                .WithMany(u => u.Planets)
                .HasPrincipalKey(u => u.UserName)
                .HasForeignKey(w => w.CreatorUsername);
            
            world.HasIndex(w => new { w.CreatorUsername, w.Name })
                .IsUnique();

            world.OwnsOne(w => w.LandColor);
            world.OwnsOne(w => w.AntiLandColor);
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
        });
    }

    public DbSet<Planet> Planets => Set<Planet>();
    public DbSet<Continent> Continents => Set<Continent>();
    public DbSet<Region> Regions => Set<Region>();
}
