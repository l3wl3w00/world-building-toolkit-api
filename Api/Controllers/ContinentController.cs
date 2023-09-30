using Bll.Continent.Dto;
using Bll.Continent.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class ContinentController(IContinentService continentService) : ControllerBase
{
    [HttpPost("planet/{planetid:guid}/continent")]
    public async Task<ActionResult<ContinentDto>> CreateContinent(Guid planetId, CreateContinentDto createContinentDto)
    {
        return await continentService.Create(planetId, createContinentDto);
    }
}