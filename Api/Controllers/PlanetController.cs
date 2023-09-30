using System.Collections;
using Bll.Auth;
using Bll.Common;
using Bll.Common.Extension;
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
public class PlanetController(IPlanetService planetService) : ControllerBase
{
    [HttpGet("{guid:guid}")]
    [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<PlanetDto>> Get(Guid guid)
    {
        return await planetService.Get(guid);
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ICollection<PlanetSummaryDto>>> GetAllForUser()
    {
        
        var worlds =  await planetService.GetAll(HttpContext.GetUserEntity());

        return Ok(worlds);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PlanetDto>> Create([FromBody] CreatePlanetDto createPlanetDto)
    {
        return await planetService.Create(createPlanetDto, HttpContext.GetUserEntity());
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await planetService.Delete(id);
        return NoContent();
    }
    
        
    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<PlanetSummaryDto>> UpdateNameAndDescription(Guid id, [FromBody] CreatePlanetDto updatePlanetDto)
    {
        var response = await planetService.UpdateNameAndDescription(id, HttpContext.GetUserEntity(), updatePlanetDto);
        return Ok(response);
    }
}
