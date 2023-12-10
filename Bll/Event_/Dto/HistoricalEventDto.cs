namespace Bll.Event_.Dto;

public record HistoricalEventDto(
    Guid Id,
    string Name, 
    string Description, 
    DateInstanceDto RelativeStart, 
    DateInstanceDto RelativeEnd,
    Guid DefaultCalendarId,
    Guid RegionId);
    
public record CreateHistoricalEventDto(
    string Name, 
    string Description, 
    DateInstanceDto RelativeStart, 
    DateInstanceDto RelativeEnd,
    Guid DefaultCalendarId);
public record DateInstanceDto(
    uint Year, 
    string YearPhase, 
    uint Day, 
    uint Hour = 0,
    uint Minute = 0);