using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Bll.Auth;
using Bll.Auth.Dto;
using Bll.Auth.Exception;
using Bll.Auth.Exception.Helper;
using Bll.Common.Exception;
using Bll.Common.Extension;
using Bll.Common.Result_;
using Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bll.User_;
using UserEntity = Dal.Entities.User;

public class UserService(
        WorldBuilderDbContext dbContext,
        IMapper mapper,
        UserManager<UserEntity> userManager,
        RegisterErrorExceptionMapper registerErrorHandler)
    : IUserService
{
    public async Task<Result<UserIdentityDto>> Create(RegisterDto registerDto)
    {
        var user = mapper.Map<UserEntity>(registerDto);
        
        var createdResult = await Create(user, registerDto.Password);
        if (createdResult.IsErrorOut(out var created)) return createdResult.Into<UserIdentityDto>();
        
        return mapper.Map<UserIdentityDto>(created).ToOkResult();
    }

    private async Task<Result<UserEntity>> Create(UserEntity user, string? password = null)
    {
        try
        {
            var result = password switch
            {
                null => await userManager.CreateAsync(user),
                _ => await userManager.CreateAsync(user, password),
            };
            if (result.Succeeded) return user.ToOkResult();
            return registerErrorHandler.ToException(result.Errors).ToErrorResult<UserEntity>();
        }
        catch (DbUpdateException)
        {
            return EntityAlreadyExistsException.Create<UserEntity>().ToErrorResult<UserEntity>();
        }
    }

    public async Task<Result<UserEntity>> GetOrCreateUser(GoogleIdentity googleIdentity)
    {
        var getByEmailResult = await GetByEmail(googleIdentity.Email);
        if (!getByEmailResult.IsErrorOf<EntityNotFoundException>()) return getByEmailResult;
        var createResult = await Create(CreateUserInMemory(googleIdentity));
        return createResult;
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
    public async Task<Result<UserEntity>> FindUserByLogin(LoginDto loginDto) =>
        loginDto.LoginType switch
        {
            LoginType.ByUsername => await GetByUsername(loginDto.UsernameOrEmail),
            LoginType.ByEmail => await GetByEmail(loginDto.UsernameOrEmail),
            _ => new NotSupportedLoginTypeException(loginDto.LoginType).ToErrorResult<UserEntity>(),
        };

    private async Task<Result<UserEntity>> GetByUsername(string username)
    {
        return await GetBy(u => u.UserName!, username, nameof(username));
    }

    public async Task<Result<UserEntity>> GetByEmail(string email)
    {
        return await GetBy(u => u.Email!, email, nameof(email));
    }

    private async Task<Result<UserEntity>> GetBy<T>(Expression<Func<UserEntity, T>> userProperty, T value, string valueName) where T : class
    {
        var equality = Expression.Equal(userProperty.Body, Expression.Constant(value, typeof(T)));

        return await dbContext.Users
            .Where(Expression.Lambda<Func<UserEntity, bool>>(equality, userProperty.Parameters))
            .SingleOrError(EntityNotFoundException.Create<UserEntity>(valueName, value));
    }
}