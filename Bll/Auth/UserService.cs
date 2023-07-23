using AutoMapper;
using Bll.Common.Exception;
using Dal;
using Dal.Entities;
using Bll.Auth.Dto;
using Bll.Auth.Exception;
using Bll.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bll.Auth;

public class UserService : IUserService
{
    private readonly WorldBuilderDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RegisterErrorExceptionMapper _registerErrorHandler;
    private readonly IMapper _mapper;

    public UserService(WorldBuilderDbContext dbContext, IMapper mapper, UserManager<User> userManager, RegisterErrorExceptionMapper registerErrorHandler)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userManager = userManager;
        _registerErrorHandler = registerErrorHandler;
    }

    public async Task<UserIdentityDto> Login(LoginDto loginDto)
    {
        var user = await FindUserByLogin(loginDto);
        var result = _userManager.PasswordHasher.VerifyHashedPassword(user,user.PasswordHash!, loginDto.Password);
        if (result == PasswordVerificationResult.Success)
            return _mapper.Map<UserIdentityDto>(user);
        throw new LoginException();
    }

    private async Task<User> FindUserByLogin(LoginDto loginDto)
    {
        if (loginDto.LoginType == LoginType.ByUsername)
        {
            var username = loginDto.UsernameOrEmail;
            return await _dbContext.Users
                .Where(u => u.UserName == username)
                .SingleOrDo(() => EntityNotFoundException.Throw<User>("username", username));
        }
        if (loginDto.LoginType == LoginType.ByEmail)
        {
            var email = loginDto.UsernameOrEmail;
            return await _dbContext.Users
                .Where(u => u.Email == email)
                .SingleOrDo(() => EntityNotFoundException.Throw<User>("email", email));
        }
        
        throw new NotSupportedLoginTypeException(loginDto.LoginType);
        
    }

    public async Task<UserIdentityDto> Register(RegisterDto registerDto)
    {
        var user = _mapper.Map<User>(registerDto);
        try
        {
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded) return _mapper.Map<UserIdentityDto>(user);
            throw _registerErrorHandler.ToException(result.Errors);
        }
        catch (DbUpdateException)
        {
            throw EntityAlreadyExistsException.Create<User>();
        }
    }
}