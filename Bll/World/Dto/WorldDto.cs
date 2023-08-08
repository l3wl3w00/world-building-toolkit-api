using Bll.Continent.Dto;

namespace Bll.World.Dto;

public record WorldDto(Guid Id, string Name, ICollection<ContinentDto> Continents, string Description = "");