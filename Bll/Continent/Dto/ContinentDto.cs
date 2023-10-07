using Bll.Common.Option;

namespace Bll.Continent.Dto;

public record ContinentDto(
    Guid Id,
    string Name,
    string Description,
    bool Inverted,
    List<PlanetCoordinateDto> Bounds);

public record ContinentPatchDto(string? Name, string? Description, bool? Inverted);

public record CreateContinentDto(
    string Name,
    string Description, 
    Guid? ParentContinentId,
    List<PlanetCoordinateDto> Bounds);