namespace Dal.Entities;

public class HistoricalEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public GlobalTimeInstance Start { get; set; } = null!;
    public GlobalTimeInstance End { get; set; } = null!;
    public Guid RegionId { get; set; }
    public Region Region { get; set; } = null!;
}

public record GlobalTimeInstance(TimeSpan Time);
