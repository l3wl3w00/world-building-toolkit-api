using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dal;
using Microsoft.EntityFrameworkCore;
using Bll.Common.Exception;
using Bll.Common.Extension;
using Bll.Common.Option;
using Bll.Planet.Dto;
using Microsoft.Data.SqlClient;
using Planet = Dal.Entities.Planet;

namespace Bll.Planet;

public class PlanetService(WorldBuilderDbContext dbContext, IMapper mapper) : IPlanetService
{
    public async Task<ICollection<PlanetSummaryDto>> GetAll(Dal.Entities.User user)
    {
        return await dbContext.Planets
            .Where(p => p.CreatorUsername == user.UserName)
            .ProjectTo<PlanetSummaryDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<PlanetDto> Get(Guid guid)
    {
        return await dbContext.Planets
            .Where(p => p.Id == guid)
            .ProjectTo<PlanetDto>(mapper.ConfigurationProvider)
            .SingleOrDo(() => throw EntityNotFoundException.Create<Dal.Entities.Planet>(guid));
    }
    
    public async Task<PlanetDto> Create(CreatePlanetDto createPlanetDto, Dal.Entities.User creator)
    {
        var planet = mapper.Map<Dal.Entities.Planet>(createPlanetDto);
        planet.CreatorUsername = creator.UserName!;
        dbContext.Planets.Add(planet);
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sqlEx)
        {
            switch (sqlEx.Number)
            {
                case SqlErrorNumbers.DuplicateUniqueConstraintViolation:
                    throw EntityAlreadyExistsException.Create<Dal.Entities.Planet>();
                case SqlErrorNumbers.ForeignKeyViolation:
                    throw new EntityNotFoundException($"No user was found with the username {creator.UserName}");
            }

            throw;
        }
        return mapper.Map<PlanetDto>(planet);
    }

    public async Task<bool> Delete(Guid id)
    {
        var planetToDelete = await FindOrThrowDoesntExist(id);

        dbContext.Planets.Remove(planetToDelete);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PlanetSummaryDto> UpdateNameAndDescription(Guid id, Dal.Entities.User user,
        CreatePlanetDto updatePlanetDto)
    {
        var planetToUpdate = await FindOrThrowDoesntExist(id);
        var alreadyExits = (await FindByName(updatePlanetDto.Name, user)).HasValue;
        if (alreadyExits) throw EntityAlreadyExistsException.Create<Dal.Entities.Planet>();
        
        planetToUpdate.Name = updatePlanetDto.Name;
        planetToUpdate.Description = updatePlanetDto.Description;

        dbContext.Planets.Update(planetToUpdate);
        await dbContext.SaveChangesAsync();
        return mapper.Map<PlanetSummaryDto>(planetToUpdate);
    }

    private Task<Dal.Entities.Planet> FindOrThrowDoesntExist(Guid id)
    {
        return dbContext.Planets
            .Where(p => p.Id == id)
            .SingleOrDo(() => throw EntityNotFoundException.Create<Dal.Entities.Planet>(id));
    }
    
    private Task<Option<Dal.Entities.Planet>> FindByName(string name, Dal.Entities.User user)
    {
        return 
            dbContext.Planets
            .Where(p => p.Name == name && p.CreatorUsername == user.UserName)
            .FirstOrOptionAsync();
    }
}