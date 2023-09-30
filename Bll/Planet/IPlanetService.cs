using Bll.Planet.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bll.Planet;

public interface IPlanetService
{
    Task<ICollection<PlanetSummaryDto>> GetAll(Dal.Entities.User user);
    Task<PlanetDto> Get(Guid guid);
    Task<PlanetDto> Create(CreatePlanetDto createPlanetDto, Dal.Entities.User creator);
    Task<bool> Delete(Guid id);
    Task<PlanetSummaryDto> UpdateNameAndDescription(Guid id, Dal.Entities.User user,
        CreatePlanetDto updatePlanetDto);
}