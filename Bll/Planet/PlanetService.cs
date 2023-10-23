﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dal;
using Microsoft.EntityFrameworkCore;
using Bll.Common.Exception;
using Bll.Common.Extension;
using Bll.Common.Option;
using Bll.Continent.Dto;
using Bll.Continent.Service;
using Bll.Planet.Dto;
using Microsoft.Data.SqlClient;
using Planet = Dal.Entities.Planet;

namespace Bll.Planet;

public class PlanetService(WorldBuilderDbContext dbContext, IMapper mapper, IContinentService continentService) : IPlanetService
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
        var planet = await dbContext.Planets
            .Where(p => p.Id == guid)
            .Include(p => p.Continents)
                .ThenInclude(c => c.Regions)
            .SingleOrDo(() => throw EntityNotFoundException.Create<Dal.Entities.Planet>(guid));
        return mapper.Map<PlanetDto>(planet); // because .ProjectTo<PlanetDto>(mapper.ConfigurationProvider) produces incorrect result
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

        await continentService.Create(
            planet.Id, 
            new CreateContinentDto(
                Name: "Root Continent",
                Description: "",
                ParentContinentId: null,
                Bounds: new List<PlanetCoordinateDto>()
                ));
        
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
        PlanetPatchDto planetPatchDto)
    {
        var planetToUpdate = await FindOrThrowDoesntExist(id);
        var alreadyExits = (await FindByName(planetPatchDto.Name, user)).HasValue;
        if (alreadyExits) throw EntityAlreadyExistsException.Create<Dal.Entities.Planet>();

        mapper.Map(planetPatchDto, planetToUpdate);
        
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