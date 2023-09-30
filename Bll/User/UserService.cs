using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Bll.Auth;
using Bll.Common.Exception;
using Dal;
using Dal.Entities;
using Bll.Auth.Dto;
using Bll.Auth.Exception;
using Bll.Auth.Exception.Helper;
using Bll.Common;
using Bll.Common.Extension;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Bll.User;
using UserEntity = Dal.Entities.User;

public class UserService(
        WorldBuilderDbContext dbContext,
        IMapper mapper,
        UserManager<UserEntity> userManager,
        RegisterErrorExceptionMapper registerErrorHandler)
    : IUserService
{
    public async Task<UserIdentityDto> Create(RegisterDto registerDto)
    {
        var user = mapper.Map<UserEntity>(registerDto);
        try
        {
            return mapper.Map<UserIdentityDto>(await Create(user, registerDto.Password));
        }
        catch (DbUpdateException)
        {
            throw EntityAlreadyExistsException.Create<UserEntity>();
        }
    }

    private async Task<UserEntity> Create(UserEntity user, string? password = null)
    {

        IdentityResult result;
        if (password is null)
        {
            result = await userManager.CreateAsync(user);
        }
        else
        {
            result = await userManager.CreateAsync(user, password);
        }
        if (result.Succeeded) return user;
        throw registerErrorHandler.ToException(result.Errors);
    }

    public async Task<UserEntity> GetOrCreateUser(GoogleIdentity googleIdentity)
    {
        try
        {
            return await GetByEmail(googleIdentity.Email);
        }
        catch (EntityNotFoundException)
        {
            return await Create(CreateUserInMemory(googleIdentity));
        }
    }

    private static UserEntity CreateUserInMemory(GoogleIdentity googleIdentity)
    {
        var name = string.Join("", googleIdentity.Name
            .Normalize(NormalizationForm.FormD)
            .Where(c =>  CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .Where(c => c != ' ')
            .ToList());
        return new UserEntity { Email = googleIdentity.Email, UserName = name};
    } 
    public async Task<UserEntity> FindUserByLogin(LoginDto loginDto)
    {
        if (loginDto.LoginType == LoginType.ByUsername)
        {
            var username = loginDto.UsernameOrEmail;
            return await GetByUsername(username);
        }
        if (loginDto.LoginType == LoginType.ByEmail)
        {
            var email = loginDto.UsernameOrEmail;
            return await GetByEmail(email);
        }
        
        throw new NotSupportedLoginTypeException(loginDto.LoginType);
        
    }

    private async Task<UserEntity> GetByUsername(string username)
    {
        return await GetBy(u => u.UserName!, username, nameof(username));

    }

    public async Task<UserEntity> GetByEmail(string email)
    {
        return await GetBy(u => u.Email!, email, nameof(email));
    }

    private async Task<UserEntity> GetBy<T>(Expression<Func<UserEntity, T>> userProperty, T value, string valueName) where T : class
    {
        var equality = Expression.Equal(userProperty.Body, Expression.Constant(value, typeof(T)));

        return await dbContext.Users
            .Where(Expression.Lambda<Func<UserEntity, bool>>(equality, userProperty.Parameters))
            .SingleOrDo(() => EntityNotFoundException.Throw<UserEntity>(valueName, value));
    }
}