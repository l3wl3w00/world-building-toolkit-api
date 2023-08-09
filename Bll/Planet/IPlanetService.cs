using Bll.Planet.Dto;

namespace Bll.Planet;

public interface IPlanetService
{
    Task<ICollection<PlanetSummaryDto>> GetAll(Dal.Entities.User user);
    Task<PlanetDto> Get(Guid guid);
    Task<PlanetDto> Create(CreatePlanetDto createPlanetDto);
}