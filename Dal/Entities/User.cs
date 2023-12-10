using Microsoft.AspNetCore.Identity;

namespace Dal.Entities;

public class User : IdentityUser<Guid>, IModel
{
    public ICollection<Planet> Planets { get; set; } = new List<Planet>();
}