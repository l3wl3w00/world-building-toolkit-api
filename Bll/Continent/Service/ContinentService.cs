using System.Collections.Immutable;
using AutoMapper;
using Bll.Common.Exception;
using Bll.Common.Extension;
using Bll.Common.Option;
using Bll.Continent.Dto;
using Dal;
using Dal.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bll.Continent.Service;

public class ContinentService(
        WorldBuilderDbContext worldBuilderDbContext,
        IMapper mapper)
    : IContinentService
{
    public async Task<ContinentDto> Create(Guid planetId, CreateContinentDto createContinentDto)
    {
        var continent = mapper.Map<Dal.Entities.Continent>(createContinentDto);
        var planet = await worldBuilderDbContext.Planets
            .SingleOrOptionAsync(p => p.Id == planetId)
            .AssertNotNullAsync(EntityNotFoundException.Create<Dal.Entities.Planet>(continent.PlanetId));
        continent.PlanetId = planetId;

        await createContinentDto.ParentContinentId
            .ToOption()
            .DoIfNotNullAsync(id => AssertContinentExists(planet.Id, id));
        
        worldBuilderDbContext.Continents.Add(continent);
        
        await worldBuilderDbContext.SaveChangesAsync();
        
        return mapper.Map<ContinentDto>(continent);
    }

    private async Task AssertContinentExists(Guid planetId, Guid continentId)
    {
        await worldBuilderDbContext.Continents
            .Where(c => c.PlanetId == planetId)
            .SingleOrOptionAsync(c => c.Id == continentId)
            .AssertNotNullAsync(new EntityNotFoundException(
                $"No {nameof(Dal.Entities.Continent)} with id {continentId} " +
                $"was found on {nameof(Dal.Entities.Planet)} with id {planetId}"));
    }

    public async Task<ActionResult<ContinentDto>> Invert(Guid continentId)
    {
        var continent = await worldBuilderDbContext.Continents
            .SingleOrOptionAsync(c => c.Id == continentId)
            .AssertNotNullAsync(EntityNotFoundException.Create<Dal.Entities.Continent>(continentId));

        continent.Inverted = !continent.Inverted;
        await worldBuilderDbContext.SaveChangesAsync();
        return mapper.Map<ContinentDto>(continent);
    }

    public async Task<ActionResult<ContinentDto>> ApplyPatch(Guid continentId, ContinentPatchDto patch)
    {
        var continentToUpdate = await worldBuilderDbContext.Continents
            .SingleOrOptionAsync(c => c.Id == continentId)
            .AssertNotNullAsync(EntityNotFoundException.Create<Dal.Entities.Continent>(continentId));
        
        mapper.Map(patch, continentToUpdate);

        worldBuilderDbContext.Continents.Update(continentToUpdate);
        
        await worldBuilderDbContext.SaveChangesAsync();

        return mapper.Map<ContinentDto>(continentToUpdate);
    }
    
}