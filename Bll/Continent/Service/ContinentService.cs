using AutoMapper;
using Bll.Common.Exception;
using Bll.Common.Option;
using Bll.Continent.Dto;
using Dal;
using Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bll.Continent.Service;

public class ContinentService(
        WorldBuilderDbContext worldBuilderDbContext,
        IMapper mapper)
    : IContinentService
{
    public async Task<ContinentDto> Create(Guid planetId, CreateContinentDto createContinentDto)
    {
        var continent = mapper.Map<Dal.Entities.Continent>(createContinentDto);
        continent.PlanetId = planetId;
        await worldBuilderDbContext.Continents.AddAsync(continent);
        try
        {
            await worldBuilderDbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw EntityNotFoundException.Create<Dal.Entities.Planet>(continent.PlanetId);
        }
        return mapper.Map<ContinentDto>(continent);
    }

    public async Task<ActionResult<ContinentDto>> Invert(Guid continentId)
    {
        Option<Dal.Entities.Continent> continent = await worldBuilderDbContext.Continents
            .SingleOrDefaultAsync(c => c.Id == continentId);
        if (continent.NoValue) 
            throw EntityNotFoundException.Create<Dal.Entities.Continent>(continentId);

        continent.Value.Inverted = !continent.Value.Inverted;
        await worldBuilderDbContext.SaveChangesAsync();
        return mapper.Map<ContinentDto>(continent);
    }
}