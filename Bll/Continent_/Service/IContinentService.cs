using Bll.Continent_.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bll.Continent_.Service;

public interface IContinentService
{
    Task<ContinentDto> Create(Guid planetId, CreateContinentDto createContinentDto);
    Task<ActionResult<ContinentDto>> Invert(Guid continentId);
    Task<ActionResult<ContinentDto>> ApplyPatch(Guid continentId,
        ContinentPatchDto patch);
}