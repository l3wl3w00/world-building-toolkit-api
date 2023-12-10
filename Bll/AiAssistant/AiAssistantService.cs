using System.Text;
using AutoMapper;
using Bll.AiAssistant.Dto;
using Bll.Common.Exception;
using Bll.Common.Extension;
using Bll.Common.Result_;
using Bll.Planet_;
using Bll.Planet_.Dto;
using Dal;
using Dal.Entities;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenAI_API;

namespace Bll.AiAssistant;

public class AiAssistantService(IOpenAIAPI openai, IPlanetQueryService planetQueryService, IMapper mapper) : IAiAssistantService
{
    public async Task<Result<AiAnswerDto>> SendPrompt(Guid planetId, AiPromptDto aiPromptDto)
    {
        var planetOpt = await planetQueryService.GetEntityAllIncluded(planetId);
        if (planetOpt.NoValueOut(out var planet))
        {
            return EntityNotFoundException.Create<Planet>().ToErrorResult<AiAnswerDto>();
        }
        
        var prompt = GeneratePrompt(planet!, aiPromptDto);

        var answer = new StringBuilder();
        
        await foreach (var result in openai.Completions.StreamCompletionEnumerableAsync(prompt, model:"gpt-4.0-text-davinci", max_tokens: 2500))
        {
            answer.Append(string.Join("", result.Completions.Select(c => c.Text)));
        }
        
        return new AiAnswerDto(answer.ToString()).ToOkResult();
    }

    private string GeneratePrompt(Planet planet, AiPromptDto aiPromptDto)
    {
        var planetDto = planetQueryService.ToPlanetDto(planet!);
        var planetForAi = mapper.Map<PlanetForAiPromptDto>(planetDto);

        var prompt = new StringBuilder(
            @"(
            A user is currently making an imaginary world for a fictional story. 
            Answer their prompt in regards to this world.
            Below I will provide the current continents, regions and events that are present on the planet
            The continents are arranged in a tree. The root continent is an ocean. Any continents below it are land continents. Any continents inside a land continent are water continents, and so on.
            Regions can be attached to a continent, which define an arbitrary polygon over the surface of the sphere.
            Historical events can be attached to a region, these events indicate the passing of time and the happenings inside the world
            There are multiple calendars as the calendar on earth depends on how much time it takes for the planet to orbit the sun,
            how long a day takes and also the specific culture. The dates for the events are given in the context of a calendar
            Your answer should only include the response to the user and nothing else, and it should be written as if you are directly answering them

            Here is the data for the world in json format:");
        
        var json = JsonConvert.SerializeObject(planetForAi);
        prompt.AppendLine()
            .AppendLine(json)
            .AppendLine()
            .AppendLine("The prompt of the user is:")
            .AppendLine(aiPromptDto.Prompt);
        return prompt.ToString();
    }
}