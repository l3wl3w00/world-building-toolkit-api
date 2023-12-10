using Bll.Calendar_.Dto;
using Bll.Common.Option_;
using Bll.Common.Result_;
using Bll.Event_.Dto;
using Dal.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bll.Calendar_;

public interface ICalendarService
{
    Task<Result<CalendarDto>> Create(Guid planetId, CreateCalendarDto calendarDto);
    Task<Option<CalendarDto>> GetDefault(Guid planetId);
    Task<Result<CalendarDto>> CreateDefaultForPlanet(Planet planet);
    Task<Result<CalendarDto>> Get(Guid calendarId);
    Task<ICollection<CalendarDto>> GetAllForPlanet(Guid planetId);
    Task<Result<CalendarDto>> Delete(Guid planetId, string calendarToDeleteName);
    Task<Result<GlobalTimeInstance>> ResolveDate(Guid calendarId, DateInstanceDto start);
    Task<Result<DateInstanceDto>> Convert(Guid planetId, string sourceCalendarName, string targetCalendarName, DateInstanceDto date);
}