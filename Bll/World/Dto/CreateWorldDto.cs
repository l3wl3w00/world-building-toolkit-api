namespace Bll.World.Dto;

public record CreateWorldDto(string Name, string CreatorUsername, string Description = "");