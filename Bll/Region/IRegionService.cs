using Bll.Continent.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bll.Region;

public interface IRegionService
{
    Task<RegionDto> Create(Guid continentId, CreateRegionDto dto);
    Task<RegionDto> Get(Guid regionId);
}