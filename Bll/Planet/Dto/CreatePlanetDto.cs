using Bll.Common.Extension;
using Bll.Continent.Dto;

namespace Bll.Planet.Dto;

public record CreatePlanetDto(string Name, string Description = "")
{
    private readonly string _name = Name;

    public string Name
    {
        get => _name;
        init => _name = value.StripBlanks();
    }
}

public record PlanetDto(Guid Id, string Name, ICollection<ContinentDto> Continents, string Description = "");
public record PlanetSummaryDto(Guid Id, string Name);
