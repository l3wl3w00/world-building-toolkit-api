using Bll.Common.Option_;
using Bll.Common.Result_;
using Bll.Planet_.Dto;
using Dal.Entities;

namespace Bll.Planet_;

public interface IPlanetModifierService
{
    Task<PlanetDto> Create(CreatePlanetDto createPlanetDto, User creator);
    Task<bool> Delete(Guid id);
    Task<PlanetSummaryDto> Patch(Guid id, User user, PlanetPatchDto planetPatchDto);
}

public interface IPlanetQueryService
{
    Task<ICollection<PlanetSummaryDto>> GetAll(User user);
    Task<Result<PlanetDto>> Get(Guid guid);
    Task<Option<Planet>> GetWithCalendarsIncludedOrOption(Guid guid);
    PlanetDto ToPlanetDto(Planet planet);
    Task<Option<Planet>> GetEntityAllIncluded(Guid guid);
}