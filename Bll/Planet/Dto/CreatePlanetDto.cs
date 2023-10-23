using Bll.Common.Extension;
using Bll.Continent.Dto;
using Dal.Entities;

namespace Bll.Planet.Dto;

public record CreatePlanetDto(
    string Name, 
    Color LandColor, 
    Color AntiLandColor,    
    Guid CalendarId,
    string Description = "")
{
    private readonly string _name = Name;

    public string Name
    {
        get => _name;
        init => _name = value.StripBlanks();
    }
}

public record PlanetPatchDto(string? Name, Color? LandColor, Color? AntiLandColor, string? Description, Guid? CalendarId)
{
    private readonly string? _name = Name;

    public string? Name
    {
        get => _name;
        init => _name = value?.StripBlanks();
    }
}

public record PlanetDto(
    Guid Id,
    string Name,
    ICollection<ContinentDto> Continents,
    Color LandColor,
    Color AntiLandColor,
    Guid CalendarId,
    string Description = "");
public record PlanetSummaryDto(Guid Id, string Name);
