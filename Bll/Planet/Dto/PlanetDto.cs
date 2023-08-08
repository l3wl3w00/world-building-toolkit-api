using Bll.Continent.Dto;

namespace Bll.Planet.Dto;

public record PlanetDto(Guid Id, string Name, ICollection<ContinentDto> Continents, string Description = "");