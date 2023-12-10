using Bll.Region_;
using Bll.Region_.Dto;
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
    
    [HttpPatch("region/{regionId:guid}")]
    [Authorize]
    public async Task<ActionResult<RegionDto>> Create(Guid regionId, [FromBody] RegionPatchDto patchDto)
    {
        return await regionService.ApplyPatch(regionId, patchDto);
    }
    
    [HttpGet("region/{regionId:guid}")]
    [Authorize]
    public async Task<ActionResult<RegionDto>> Get(Guid regionId)
    {
        return await regionService.Get(regionId);
    }
}