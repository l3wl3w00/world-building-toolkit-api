using Bll.Continent_.Dto;
using Bll.Event_.Dto;
using Dal.Entities;
using Color = Dal.Entities.Color;

namespace Bll.Region_.Dto;

public record RegionDto(
    Guid Id,
    Guid ContinentId,
    string Name,
    string Description,
    bool Inverted,
    Color Color,
    RegionType RegionType,
    List<HistoricalEventDto> Events,
    List<PlanetCoordinateDto> Bounds);
    
    
public record CreateRegionDto(
    string Name,
    string Description,
    RegionType RegionType,
    Color Color,
    List<PlanetCoordinateDto> Bounds);
    
        
public record RegionPatchDto(
    string? Name,
    string? Description,
    bool? Inverted,
    Color? Color,
    RegionType? RegionType);
    
public record RegionForAiPromptDto(
    Guid Id,
    Guid ContinentId,
    string Name,
    string Description,
    Color Color,
    RegionType RegionType,
    List<HistoricalEventDto> Events);
