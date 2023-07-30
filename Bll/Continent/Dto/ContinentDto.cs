namespace Bll.Continent.Dto;

public record ContinentDto(Guid Id, string Name, string? Description, List<WorldCoordinateDto> Bounds);