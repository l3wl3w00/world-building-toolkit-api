using Bll.AiAssistant;
using Bll.AiAssistant.Dto;
using Bll.Common.Result_;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class AiAssistantController(IAiAssistantService aiService) : ControllerBase
{
    [HttpPost("planet/{planetid:guid}/askAi")]
    [Authorize]
    public async Task<ActionResult<AiAnswerDto>> Ask(Guid planetId, [FromBody] AiPromptDto aiPromptDto)
    {
        var result = await aiService.SendPrompt(planetId, aiPromptDto);
        return result.ThrowIfError();
    }
}