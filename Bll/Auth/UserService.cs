using AutoMapper;
using Bll.Exception;
using Dal;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Bll.Auth.Dto;

namespace Bll.Auth;

public class UserService : IUserService
{
    private readonly WorldBuilderDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserService(WorldBuilderDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<UserIdentityDto> Register(RegisterDto registerDto)
    {
        var user = _mapper.Map<User>(registerDto);
        _dbContext.Users.Add(user);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            EntityAlreadyExistsException.Throw<User>();
        }

        return _mapper.Map<UserIdentityDto>(user);
    }
}