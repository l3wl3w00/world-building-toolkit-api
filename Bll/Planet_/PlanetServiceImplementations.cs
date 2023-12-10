using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bll.Calendar_;
using Bll.Common.Exception;
using Bll.Common.Extension;
using Bll.Common.Option_;
using Bll.Common.Result_;
using Bll.Continent_.Dto;
using Bll.Continent_.Service;
using Bll.Event_.Dto;
using Bll.Planet_.Dto;
using Dal;
using Dal.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Bll.Planet_;

public class PlanetModifierService(
    WorldBuilderDbContext dbContext,
    IMapper mapper,
    IContinentService continentService,
    ICalendarService calendarService) : IPlanetModifierService
{

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
                    throw EntityAlreadyExistsException.Create<Planet>();
                case SqlErrorNumbers.ForeignKeyViolation:
                    throw new EntityNotFoundException(typeof(User), $"No user was found with the username {creator.UserName}");
            }

            throw;
        }

        await this.CreateRootContinent(planet);
        await calendarService.CreateDefaultForPlanet(planet);
        var m = mapper;
        return mapper.Map<PlanetDto>(planet);
    }

    private async Task CreateRootContinent(Dal.Entities.Planet planet)
    {
        await continentService.Create(
            planet.Id,
            new CreateContinentDto(
                Name: "Root Continent",
                Description: "",
                ParentContinentId: null,
                Bounds: new List<PlanetCoordinateDto>()
            ));
    }

    public async Task<bool> Delete(Guid id)
    {
        var planetToDelete = await FindOrThrowDoesntExist(id);

        dbContext.Planets.Remove(planetToDelete);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PlanetSummaryDto> Patch(Guid id, Dal.Entities.User user,
        PlanetPatchDto planetPatchDto)
    {
        var planetToUpdate = await FindOrThrowDoesntExist(id);
        if (planetPatchDto.Name != null)
        {
            var alreadyExits = (await FindByName(planetPatchDto.Name, user)).HasValue;
            if (alreadyExits) throw EntityAlreadyExistsException.Create<Dal.Entities.Planet>();
        }
        
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

public class PlanetQueryService(WorldBuilderDbContext dbContext, IMapper mapper) : IPlanetQueryService
{
    public async Task<ICollection<PlanetSummaryDto>> GetAll(User user)
    {
        return await dbContext.Planets
            .Where(p => p.CreatorUsername == user.UserName)
            .ProjectTo<PlanetSummaryDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Option<Planet>> GetEntityAllIncluded(Guid guid)
    {
        return await dbContext.Planets
            .Where(p => p.Id == guid)
            .Include(p => p.Calendars).ThenInclude(c => c.Events)
            .Include(p => p.Continents).ThenInclude(c => c.Regions)
            .SingleOrOptionAsync();
    }
    
    public async Task<Result<PlanetDto>> Get(Guid guid)
    {
        var planetOpt = await GetEntityAllIncluded(guid);
        if (planetOpt.NoValueOut(out var planet))
        {
            return EntityNotFoundException.Create<Planet>(guid).ToErrorResult<PlanetDto>();
        }
        return ToPlanetDto(planet!).ToOkResult();
    }

    public PlanetDto ToPlanetDto(Planet planet)
    {
        foreach (var calendar in planet.Calendars)
        {
            foreach (var historicalEvent in calendar.Events)
            {
                historicalEvent.RelativeStart = calendar.ToRelative(historicalEvent.Start);
                historicalEvent.RelativeEnd = calendar.ToRelative(historicalEvent.End);
            }
        }
        
        return mapper.Map<PlanetDto>(planet);
    }
    public async Task<Option<Dal.Entities.Planet>> GetWithCalendarsIncludedOrOption(Guid guid)
    {
        return await dbContext.Planets
            .Where(p => p.Id == guid)
            .Include(p => p.Calendars)
            .ToOptionAsync();
    }

}