using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bll.Common;
using Dal;
using Microsoft.EntityFrameworkCore;
using Bll.Common.Exception;
using Bll.World.Dto;
using Dal.Entities;
using Microsoft.Data.SqlClient;
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

    public async Task<ICollection<WorldSummaryDto>> GetAll(Dal.Entities.User user)
    {
        return await _dbContext.Worlds
            .Where(w => w.CreatorUsername == user.UserName)
            .ProjectTo<WorldSummaryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<WorldDto> Get(Guid guid)
    {
        return await _dbContext.Worlds
            .Where(w => w.Id == guid)
            .ProjectTo<WorldDto>(_mapper.ConfigurationProvider)
            .SingleOrDo(() => throw EntityNotFoundException.Create<WorldEntity>(guid));
    }
    
    public async Task<WorldDto> Create(CreateWorldDto createWorldDto)
    {
        var world = _mapper.Map<WorldEntity>(createWorldDto);
        _dbContext.Worlds.Add(world);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sqlEx)
        {
            switch (sqlEx.Number)
            {
                case SqlErrorNumbers.DuplicateUniqueConstraintViolation:
                    throw EntityAlreadyExistsException.Create<WorldEntity>();
                case SqlErrorNumbers.ForeignKeyViolation:
                    throw new EntityNotFoundException($"No user was found with the username {createWorldDto.CreatorUsername}");
            }

            throw;
        }
        return _mapper.Map<WorldDto>(world);
    }
}