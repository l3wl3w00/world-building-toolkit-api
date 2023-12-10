using Bll.Calendar_.Dto;
using Bll.Common.Extension;
using Bll.Continent_.Dto;
using Dal.Entities;

namespace Bll.Planet_.Dto;

public record CreatePlanetDto(
    string Name, 
    Color LandColor, 
    Color AntiLandColor, 
    TimeSpan DayLength, 
    uint NumberOfDaysInYear,   
    string Description = "")
{
    private readonly string _name = Name;

    public string Name
    {
        get => _name;
        init => _name = value.StripBlanks();
    }
}

public record PlanetPatchDto(
    string? Name = null,
    Color? LandColor = null, 
    Color? AntiLandColor = null,
    string? Description = null)
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
    ICollection<CalendarDto> Calendars,
    Color LandColor,
    Color AntiLandColor,
    string Description = "");
public record PlanetSummaryDto(Guid Id, string Name);
public record PlanetForAiPromptDto(
    Guid Id,
    string Name,
    string Description, 
    List<ContinentForAiPromptDto> Continents, 
    List<CalendarDto> Calendars);
