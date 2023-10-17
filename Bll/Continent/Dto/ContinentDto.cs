using System.Collections.ObjectModel;
using Bll.Common.Option;
using Microsoft.VisualBasic;

namespace Bll.Continent.Dto;
 
public record ContinentDto(
    Guid Id,
    Guid ParentContinentId,
    string Name,
    string Description,
    bool Inverted,
    Collection<RegionDto> Regions,
    List<PlanetCoordinateDto> Bounds);

public record ContinentPatchDto(string? Name, string? Description, bool? Inverted);

public record CreateContinentDto(
    string Name,
    string Description, 
    Guid? ParentContinentId,
    List<PlanetCoordinateDto> Bounds);