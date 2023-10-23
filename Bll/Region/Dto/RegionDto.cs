using Bll.Continent.Dto;
using Dal.Entities;
using Color = Dal.Entities.Color;

namespace Bll.Region.Dto;

public record RegionDto(
    Guid Id,
    Guid ContinentId,
    string Name,
    string Description,
    bool Inverted,
    Color Color,
    RegionType RegionType,
    List<PlanetCoordinateDto> Bounds);
    
    
public record CreateRegionDto(
    string Name,
    string Description,
    RegionType RegionType,
    Color Color,
    List<PlanetCoordinateDto> Bounds);
    
        
public record RegionPatchDto(
    string Name,
    string Description,
    Color Color,
    RegionType RegionType);