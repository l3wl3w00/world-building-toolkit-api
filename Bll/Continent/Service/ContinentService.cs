using AutoMapper;
using Bll.Common.Exception;
using Bll.Continent.Dto;
using Dal;
using Dal.Entities;
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
}