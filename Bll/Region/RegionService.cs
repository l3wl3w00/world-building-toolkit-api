using AutoMapper;
using Bll.Common.Exception;
using Bll.Common.Extension;
using Bll.Common.Option;
using Bll.Continent.Dto;
using Bll.Region.Dto;
using Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bll.Region;

public class RegionService(WorldBuilderDbContext dbContext, IMapper mapper) : IRegionService
{
    private readonly WorldBuilderDbContext _dbContext = dbContext;

    public async Task<RegionDto> Create(Guid continentId, CreateRegionDto dto)
    {
        var continentOpt = await _dbContext.Continents.SingleOrOptionAsync(c => c.Id == continentId);
        var continent = continentOpt.AssertNotNull(EntityNotFoundException.Create<Dal.Entities.Continent>(continentId));

        var region = mapper.Map<Dal.Entities.Region>(dto);
        region.ContinentId = continent.Id;
        dbContext.Regions.Add(region);
        
        await dbContext.SaveChangesAsync();

        return mapper.Map<RegionDto>(region);
    }

    public async Task<RegionDto> Get(Guid regionId)
    {
        var regionOpt = await _dbContext.Regions.SingleOrOptionAsync(c => c.Id == regionId);
        var region = regionOpt.AssertNotNull(EntityNotFoundException.Create<RegionDto>(regionId));
        
        return mapper.Map<RegionDto>(region);
    }
    
    public async Task<ActionResult<RegionDto>> ApplyPatch(Guid regionId, RegionPatchDto patch)
    {
        var regionToUpdate = await dbContext.Regions
            .SingleOrOptionAsync(c => c.Id == regionId)
            .AssertNotNullAsync(EntityNotFoundException.Create<Dal.Entities.Region>(regionId));
        
        mapper.Map(patch, regionToUpdate);

        dbContext.Regions.Update(regionToUpdate);
        
        await dbContext.SaveChangesAsync();

        return mapper.Map<RegionDto>(regionToUpdate);
    }
}