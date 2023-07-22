using Bll.World;
using Microsoft.AspNetCore.Mvc;
using WorldBuilderBLL.World.Dto;

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
    public async Task<ActionResult<WorldDto>> Get(Guid guid)
    {
        return await _worldService.Get(guid);
    }

    [HttpPost]
    public async Task<ActionResult<WorldDto>> Create([FromBody] CreateWorldDto createWorldDto)
    {
        return await _worldService.Create(createWorldDto);
    }
}
