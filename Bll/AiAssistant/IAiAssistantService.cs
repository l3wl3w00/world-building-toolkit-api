using Bll.AiAssistant.Dto;
using Bll.Common.Result_;

namespace Bll.AiAssistant;

public interface IAiAssistantService
{
    Task<Result<AiAnswerDto>> SendPrompt(Guid planetId, AiPromptDto promptDto);
}