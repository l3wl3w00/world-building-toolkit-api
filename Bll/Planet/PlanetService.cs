using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bll.Common;
using Dal;
using Microsoft.EntityFrameworkCore;
using Bll.Common.Exception;
using Bll.Planet.Dto;
using Dal.Entities;
using Microsoft.Data.SqlClient;

namespace Bll.Planet;

public class PlanetService : IPlanetService
{
    private readonly WorldBuilderDbContext _dbContext;

    private readonly IMapper _mapper;

    public PlanetService(WorldBuilderDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ICollection<PlanetSummaryDto>> GetAll(Dal.Entities.User user)
    {
        return await _dbContext.Planets
            .Where(p => p.CreatorUsername == user.UserName)
            .ProjectTo<PlanetSummaryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<PlanetDto> Get(Guid guid)
    {
        return await _dbContext.Planets
            .Where(p => p.Id == guid)
            .ProjectTo<PlanetDto>(_mapper.ConfigurationProvider)
            .SingleOrDo(() => throw EntityNotFoundException.Create<Dal.Entities.Planet>(guid));
    }
    
    public async Task<PlanetDto> Create(CreatePlanetDto createPlanetDto)
    {
        var planet = _mapper.Map<Dal.Entities.Planet>(createPlanetDto);
        _dbContext.Planets.Add(planet);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sqlEx)
        {
            switch (sqlEx.Number)
            {
                case SqlErrorNumbers.DuplicateUniqueConstraintViolation:
                    throw EntityAlreadyExistsException.Create<Dal.Entities.Planet>();
                case SqlErrorNumbers.ForeignKeyViolation:
                    throw new EntityNotFoundException($"No user was found with the username {createPlanetDto.CreatorUsername}");
            }

            throw;
        }
        return _mapper.Map<PlanetDto>(planet);
    }
}