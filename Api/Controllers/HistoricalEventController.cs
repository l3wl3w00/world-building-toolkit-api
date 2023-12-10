using Bll.Common.Result_;
using Bll.Event_;
using Bll.Event_.Dto;
using Dal.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
public class HistoricalEventController(IHistoricalEventService historicalEventService)
{
    [HttpPost("region/{regionid:guid}/historicalEvent/")]
    public async Task<HistoricalEventDto> Create(Guid regionId, [FromBody] CreateHistoricalEventDto createHistoricalEventDto)
    {
        var result = await historicalEventService.Create(regionId, createHistoricalEventDto);
        return result.ThrowIfError();
    }
}