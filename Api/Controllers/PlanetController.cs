using System.Collections;
using Bll.Auth;
using Bll.Common;
using Bll.Common.Extension;
using Bll.Common.Result_;
using Bll.Planet_;
using Bll.Planet_.Dto;
using Microsoft.AspNetCore.Mvc;
using Dal.Entities;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;
[ApiController]
[Route("[controller]")]
public class PlanetController(IPlanetModifierService planetModifierService, IPlanetQueryService planetQueryService) : ControllerBase
{
    [HttpGet("{guid:guid}")]
    [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<PlanetDto>> Get(Guid guid)
    {
        var result = await planetQueryService.Get(guid);
        return result.ThrowIfError();
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ICollection<PlanetSummaryDto>>> GetAllForUser()
    {
        
        var worlds =  await planetQueryService.GetAll(HttpContext.GetUserEntity());

        return Ok(worlds);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PlanetDto>> Create([FromBody] CreatePlanetDto createPlanetDto)
    {
        return await planetModifierService.Create(createPlanetDto, HttpContext.GetUserEntity());
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        await planetModifierService.Delete(id);
        return NoContent();
    }
    
        
    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<PlanetSummaryDto>> Patch(Guid id, [FromBody] PlanetPatchDto updateDto)
    {
        var response = await planetModifierService.Patch(id, HttpContext.GetUserEntity(), updateDto);
        return Ok(response);
    }
    
    [HttpPatch("{id:guid}/calendar")]
    [Authorize]
    public async Task<ActionResult<PlanetSummaryDto>> UpdateCalendar(Guid id, [FromBody] PlanetPatchDto updateDto)
    {
        var response = await planetModifierService.Patch(id, HttpContext.GetUserEntity(), updateDto);
        return Ok(response);
    }
}
