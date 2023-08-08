namespace Bll.Planet.Dto;

public record CreatePlanetDto(string Name, string CreatorUsername, string Description = "");