using Bll.Continent.Dto;
using Bll.Continent.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class ContinentController : ControllerBase
{
    private readonly IContinentService _continentService;

    public ContinentController(IContinentService continentService)
    {
        _continentService = continentService;
    }

    [HttpPost("world/{worldid:guid}/continent")]
    public async Task<ActionResult<ContinentDto>> CreateContinent(Guid worldId, CreateContinentDto createContinentDto)
    {
        return await _continentService.Create(worldId, createContinentDto);
    }
}