namespace Bll.Continent.Dto;

public record ContinentDto(
    Guid Id,
    string Name,
    string? Description,
    bool Inverted,
    List<PlanetCoordinateDto> Bounds);

public record ContinentPatchDto(string? Name, string? Description, bool? Inverted);