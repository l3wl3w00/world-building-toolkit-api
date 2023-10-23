using Bll.Continent.Dto;
using Bll.Continent.Service;
using Bll.Region;
using Bll.Region.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class RegionController(IRegionService regionService) : ControllerBase
{
    [HttpPost("continent/{continentId:guid}/region")]
    [Authorize]
    public async Task<ActionResult<RegionDto>> Create(Guid continentId, [FromBody] CreateRegionDto createRegionDto)
    {
        return await regionService.Create(continentId, createRegionDto);
    }
    
    [HttpGet("region/{regionId:guid}")]
    [Authorize]
    public async Task<ActionResult<RegionDto>> Get(Guid regionId)
    {
        return await regionService.Get(regionId);
    }
}