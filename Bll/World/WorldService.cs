using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dal;
using Microsoft.EntityFrameworkCore;
using Bll.Exception;
using WorldBuilderBLL.World.Dto;
using WorldEntity = Dal.Entities.World;

namespace Bll.World;

public class WorldService : IWorldService
{
    private readonly WorldBuilderDbContext _dbContext;

    private readonly IMapper _mapper;

    public WorldService(WorldBuilderDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ICollection<WorldDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<WorldDto> Get(Guid guid)
    {
        var result = await _dbContext.Worlds
            .Where(w => w.Id == guid)
            .ProjectTo<WorldDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        
        if (result is null) throw new EntityNotFoundException<WorldEntity>(guid);

        return result;
    }
    
    public async Task<WorldDto> Create(CreateWorldDto createWorldDto)
    {
        var world = _mapper.Map<WorldEntity>(createWorldDto);
        _dbContext.Worlds.Add(world);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new System.Exception("todo"); //TODO
        }
        return _mapper.Map<WorldDto>(world);
    }
}