using Microsoft.AspNetCore.Identity;

namespace Dal.Entities;

public class User : IdentityUser<Guid>
{
    public ICollection<Planet> Planets { get; set; } = new List<Planet>();
}