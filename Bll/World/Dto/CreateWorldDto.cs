namespace Bll.World.Dto;

public class CreateWorldDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public string UserName { get; set; } = null!;
}