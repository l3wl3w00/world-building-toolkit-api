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
        builder.Entity<World>().HasData(
            new World { Id = Guid.NewGuid(), Name = "World 1", Description = "World 1 Description" },
            new World { Id = Guid.NewGuid(), Name = "World 2", Description = "World 2 Description" },
            new World { Id = Guid.NewGuid(), Name = "World 3", Description = "World 3 Description" }
        );
    }

    public DbSet<World> Worlds => Set<World>();
}
