using Bll.Continent_.Dto;
using Bll.Continent_.Service;
using Dal.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using JsonPatchDocument = Azure.JsonPatchDocument;

namespace Api.Controllers;

[ApiController]
public class ContinentController(IContinentService continentService) : ControllerBase
{
    [HttpPost("planet/{planetid:guid}/continent")]
    public async Task<ActionResult<ContinentDto>> Create(Guid planetId, [FromBody] CreateContinentDto createContinentDto)
    {
        return await continentService.Create(planetId, createContinentDto);
    }
    
    [HttpPatch("continent/{continentid:guid}/invert")]
    public async Task<ActionResult<ContinentDto>> InvertContinent(Guid continentId)
    {
        return await continentService.Invert(continentId);
    }
    
    [HttpPatch("continent/{continentid:guid}")]
    public async Task<ActionResult<ContinentDto>> Patch(Guid continentId, [FromBody] ContinentPatchDto patch)
    {
        return await continentService.ApplyPatch(continentId, patch);
    }
}