namespace Dal.Entities;

public class Continent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public Planet Planet { get; set; } = null!;
    public Guid PlanetId { get; set; }

    public List<PlanetCoordinate> Bounds { get; set; } = new List<PlanetCoordinate>();
}