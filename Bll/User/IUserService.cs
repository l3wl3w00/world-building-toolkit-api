using Bll.Auth;
using Bll.Auth.Dto;

using Google.Apis.Auth;

namespace Bll.User;
using UserEntity = Dal.Entities.User;

public interface IUserService
{
    Task<UserEntity> FindUserByLogin(LoginDto loginDto);
    Task<UserIdentityDto> Create(RegisterDto registerDto);
    Task<Dal.Entities.User> GetOrCreateUser(GoogleIdentity googleIdentity);
    Task<UserEntity> GetByEmail(string email);
}
