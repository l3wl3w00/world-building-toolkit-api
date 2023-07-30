namespace Dal.Entities;

public class Continent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public World World { get; set; } = null!;
    public Guid WorldId { get; set; }

    public List<WorldCoordinate> Bounds { get; set; } = new List<WorldCoordinate>();
}