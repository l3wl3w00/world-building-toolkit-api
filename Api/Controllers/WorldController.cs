using Bll.World;
using Microsoft.AspNetCore.Mvc;
using Bll.World.Dto;
using Microsoft.AspNetCore.Authentication.Google;
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
    [Authorize]
    public async Task<ActionResult<WorldDto>> Get(Guid guid)
    {
        return await _worldService.Get(guid);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<WorldDto>> Create([FromBody] CreateWorldDto createWorldDto)
    {
        return await _worldService.Create(createWorldDto);
    }
}
