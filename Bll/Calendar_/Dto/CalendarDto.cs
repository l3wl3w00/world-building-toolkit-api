using Dal.Entities;

namespace Bll.Calendar_.Dto;

public record CreateCalendarDto(string Name, string Description, ulong FirstYear, List<YearPhase> YearPhases);
public record CalendarDto(Guid Id, string Name, string Description, ulong FirstYear, Guid PlanetId, List<YearPhase> YearPhases);
