namespace Bll.Continent.Dto;

public record CreateContinentDto(string Name, string? Description, List<WorldCoordinateDto> Bounds);