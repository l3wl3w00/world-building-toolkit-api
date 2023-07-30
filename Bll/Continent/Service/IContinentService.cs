using Bll.Continent.Dto;

namespace Bll.Continent.Service;

public interface IContinentService
{
    Task<ContinentDto> Create(Guid worldId, CreateContinentDto createContinentDto);
}