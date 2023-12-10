using System.Runtime;
using Bll.Calendar_;
using Bll.Calendar_.Dto;
using Bll.Common.Option_;
using Bll.Common.Result_;
using Bll.Event_.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class CalendarController(ICalendarService calendarService) : ControllerBase 
{
    [HttpGet("calendar/{calendarid:guid}")]
    [Authorize]
    public async Task<ActionResult<CalendarDto>> Get(Guid calendarId)
    {
        var getResult = await calendarService.Get(calendarId);
        return getResult.ThrowIfError();
    }
    
    [HttpGet("planet/{planetid:guid}/calendar")]
    [Authorize]
    public async Task<ActionResult<ICollection<CalendarDto>>> GetAllForPlanet(Guid planetId)
    {
        return Ok(await calendarService.GetAllForPlanet(planetId));
    }
    
    [HttpPost("planet/{planetid:guid}/calendar")]
    [Authorize]
    public async Task<ActionResult<CalendarDto>> Create(Guid planetId, [FromBody] CreateCalendarDto createCalendarDto)
    {
        var createResult = await calendarService.Create(planetId, createCalendarDto);
        return createResult.ThrowIfError();
    }
    
        
    [HttpDelete("planet/{planetid:guid}/calendar/{calendarToDeleteName:required}")]
    [Authorize]
    public async Task<ActionResult<CalendarDto>> Delete(Guid planetId, string calendarToDeleteName)
    {
        var deleteResult = await calendarService.Delete(planetId, calendarToDeleteName);
        return deleteResult.ThrowIfError();
    }
    
    [HttpPost("planet/{planetid:guid}/calendar/convert")]
    [Authorize]
    public async Task<ActionResult<DateInstanceDto>> Convert(
        Guid planetId,
        [FromBody] DateInstanceDto dateInstanceDto,
        [FromQuery] string sourceCalendarName, 
        [FromQuery] string targetCalendarName)
    {
        var deleteResult = await calendarService.Convert(planetId, sourceCalendarName, targetCalendarName, dateInstanceDto);
        return deleteResult.ThrowIfError();
    }
}