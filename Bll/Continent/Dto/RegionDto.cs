using Dal.Entities;

namespace Bll.Continent.Dto;

public record RegionDto(
    Guid Id,
    Guid ContinentId,
    string Name,
    string Description,
    bool Inverted,
    RegionType RegionType,
    List<PlanetCoordinateDto> Bounds);
    
    
public record CreateRegionDto(
    string Name,
    string Description,
    RegionType RegionType,
    List<PlanetCoordinateDto> Bounds);
    
        
public record RegionPatchDto(
    string Name,
    string Description,
    RegionType RegionType);