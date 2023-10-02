using Bll.Continent.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Bll.Continent.Service;

public interface IContinentService
{
    Task<ContinentDto> Create(Guid planetId, CreateContinentDto createContinentDto);
    Task<ActionResult<ContinentDto>> Invert(Guid continentId);
    Task<ActionResult<ContinentDto>> ApplyPatch(Guid continentId,
        ContinentPatchDto patch);
}