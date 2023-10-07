﻿namespace Dal.Entities;

public class Continent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool Inverted { get; set; }
    public List<PlanetCoordinate> Bounds { get; set; } = new List<PlanetCoordinate>();
    public Guid PlanetId { get; set; }
    public Guid? ParentContinentId { get; set; }
    
    public Planet Planet { get; set; } = null!;
    public Continent? ParentContinent { get; set; }
}
