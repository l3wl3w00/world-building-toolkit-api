using Bll.Continent.Dto;

namespace Bll.World.Dto;

public record WorldDto(Guid Id, string Name, string? Description, ICollection<ContinentDto> Continents);