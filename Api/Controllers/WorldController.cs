using System.Collections;
using Bll.Auth;
using Bll.Common;
using Bll.World;
using Microsoft.AspNetCore.Mvc;
using Bll.World.Dto;
using Dal.Entities;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;
[ApiController]
[Route("[controller]")]
public class WorldController : ControllerBase
{
    private readonly IWorldService _worldService;

    public WorldController(IWorldService worldService)
    {
        _worldService = worldService;
    }

    [HttpGet("{guid:guid}")]
    [Authorize(AuthenticationSchemes = GoogleDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<WorldDto>> Get(Guid guid)
    {
        return await _worldService.Get(guid);
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ICollection<WorldSummaryDto>>> GetAllForUser()
    {
        
        var worlds =  await _worldService.GetAll(HttpContext.GetUserEntity());

        return Ok(worlds);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<WorldDto>> Create([FromBody] CreateWorldDto createWorldDto)
    {
        return await _worldService.Create(createWorldDto);
    }
}
