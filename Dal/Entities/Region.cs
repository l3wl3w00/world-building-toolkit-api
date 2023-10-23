namespace Dal.Entities;

public enum RegionType
{
    Country, City, Natural, Other
}
public class Region
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool Inverted { get; set; } = false;
    public RegionType RegionType { get; set; } = RegionType.Other;
    public List<PlanetCoordinate> Bounds { get; set; } = new List<PlanetCoordinate>();
    public Guid ContinentId { get; set; }
    public Color Color { get; set; } = Color.Black;
    public Continent? Continent { get; set; }
}