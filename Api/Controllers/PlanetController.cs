using System.Collections;
using Bll.Auth;
using Bll.Common;
using Bll.Planet;
using Microsoft.AspNetCore.Mvc;
using Bll.Planet.Dto;
using Dal.Entities;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;
[ApiController]
[Route("[controller]")]
public class PlanetController : ControllerBase
{
    private readonly IPlanetService _planetService;

    public PlanetController(IPlanetService planetService)
    {
        _planetService = planetService;
    }

    [HttpGet("{guid:guid}")]
    [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<PlanetDto>> Get(Guid guid)
    {
        return await _planetService.Get(guid);
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ICollection<PlanetSummaryDto>>> GetAllForUser()
    {
        
        var worlds =  await _planetService.GetAll(HttpContext.GetUserEntity());

        return Ok(worlds);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PlanetDto>> Create([FromBody] CreatePlanetDto createPlanetDto)
    {
        return await _planetService.Create(createPlanetDto);
    }
}
