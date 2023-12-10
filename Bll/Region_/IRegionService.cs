using Bll.Region_.Dto;

namespace Bll.Region_;

public interface IRegionService
{
    Task<RegionDto> Create(Guid continentId, CreateRegionDto dto);
    Task<RegionDto> ApplyPatch(Guid regionId, RegionPatchDto dto);
    Task<RegionDto> Get(Guid regionId);
}