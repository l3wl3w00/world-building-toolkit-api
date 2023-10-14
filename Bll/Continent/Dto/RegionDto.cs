using Dal.Entities;

namespace Bll.Continent.Dto;

public record RegionDto(
    Guid Id,
    Guid ContinentId,
    string Name,
    string Description,
    bool Inverted,
    List<PlanetCoordinateDto> Bounds);
    
    
public record CreateRegionDto(
    string Name,
    string Description,
    RegionType RegionType,
    List<PlanetCoordinateDto> Bounds);