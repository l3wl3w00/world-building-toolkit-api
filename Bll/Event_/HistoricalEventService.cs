using AutoMapper;
using Bll.Calendar_;
using Bll.Common.Extension;
using Bll.Common.Result_;
using Bll.Event_.Dto;
using Dal;
using Dal.Entities;

namespace Bll.Event_;

public class HistoricalEventService(WorldBuilderDbContext dbContext, IMapper mapper, ICalendarService calendarService) : IHistoricalEventService
{
    public async Task<Result<HistoricalEventDto>> Create(Guid regionId, CreateHistoricalEventDto createHistoricalEventDto)
    {
        var eventToCreate = mapper.Map<HistoricalEvent>(createHistoricalEventDto);
        
        var startResolveResult = await calendarService.ResolveDate(createHistoricalEventDto.DefaultCalendarId, createHistoricalEventDto.RelativeStart);
        if (startResolveResult.IsError) return startResolveResult.Into<HistoricalEventDto>();
        eventToCreate.StartSeconds = startResolveResult.OkValue.Seconds;

        var endResolveResult = await calendarService.ResolveDate(createHistoricalEventDto.DefaultCalendarId, createHistoricalEventDto.RelativeEnd);
        if (endResolveResult.IsError) return endResolveResult.Into<HistoricalEventDto>();
        eventToCreate.EndSeconds = endResolveResult.OkValue.Seconds;
        eventToCreate.RegionId = regionId;
        
        dbContext.Events.Add(eventToCreate);
        
        await dbContext.SaveChangesAsync();
        
        return new HistoricalEventDto(
            Id:                 eventToCreate.Id,
            Name:               eventToCreate.Name,
            Description:        eventToCreate.Description,
            RelativeStart:      createHistoricalEventDto.RelativeStart,
            RelativeEnd:        createHistoricalEventDto.RelativeEnd,
            DefaultCalendarId:  eventToCreate.DefaultCalendarId,
            RegionId:           eventToCreate.RegionId
            ).ToOkResult();
    }
}