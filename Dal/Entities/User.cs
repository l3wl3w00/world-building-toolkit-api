using Microsoft.AspNetCore.Identity;

namespace Dal.Entities;

public class User : IdentityUser<Guid>
{
    public ICollection<World> Worlds { get; set; }
}