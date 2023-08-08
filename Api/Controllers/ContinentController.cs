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

    [HttpPost("planet/{planetid:guid}/continent")]
    public async Task<ActionResult<ContinentDto>> CreateContinent(Guid planetId, CreateContinentDto createContinentDto)
    {
        return await _continentService.Create(planetId, createContinentDto);
    }
}