namespace Bll.World.Dto;

public class WorldDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}