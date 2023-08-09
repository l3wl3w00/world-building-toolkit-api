using AutoMapper;
using Bll.Common.Exception;
using Bll.Continent.Dto;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bll.Continent.Service;

public class ContinentService : IContinentService
{
    private readonly WorldBuilderDbContext _dbContext;
    private readonly IMapper _mapper;

    public ContinentService(WorldBuilderDbContext worldBuilderDbContext, IMapper mapper)
    {
        _dbContext = worldBuilderDbContext;
        _mapper = mapper;
    }

    public async Task<ContinentDto> Create(Guid planetId, CreateContinentDto createContinentDto)
    {
        var continent = _mapper.Map<Dal.Entities.Continent>(createContinentDto);
        continent.PlanetId = planetId;
        await _dbContext.Continents.AddAsync(continent);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw EntityNotFoundException.Create<Dal.Entities.Planet>(continent.PlanetId);
        }
        return _mapper.Map<ContinentDto>(continent);
    }
}