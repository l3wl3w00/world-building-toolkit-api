using Bll.Common.Result_;
using Bll.Event_.Dto;

namespace Bll.Event_;

public interface IHistoricalEventService
{
    Task<Result<HistoricalEventDto>> Create(Guid regionId, CreateHistoricalEventDto createHistoricalEventDto);
}