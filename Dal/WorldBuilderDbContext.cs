using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dal.Entities;

namespace Dal;
public class WorldBuilderDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public WorldBuilderDbContext(DbContextOptions<WorldBuilderDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<World>(world =>
        {
            world.HasOne(w => w.Creator)
                .WithMany(u => u.Worlds)
                .HasPrincipalKey(u => u.UserName)
                .HasForeignKey(w => w.CreatorUsername);
            
            world.HasIndex(w => new { w.CreatorUsername, w.Name })
                .IsUnique();
        });

        builder.Entity<User>(user =>
        {
            user.HasIndex(u => u.Email).IsUnique();
            user.HasAlternateKey(u => u.UserName);
            user.HasMany(u => u.Worlds).
                WithOne(w => w.Creator);
        });
        
    }

    public DbSet<World> Worlds => Set<World>();
    
}
