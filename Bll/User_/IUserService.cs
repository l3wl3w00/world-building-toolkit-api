using Bll.Auth;
using Bll.Auth.Dto;
using Bll.Common.Result_;

namespace Bll.User_;
using UserEntity = Dal.Entities.User;

public interface IUserService
{
    Task<Result<UserEntity>> FindUserByLogin(LoginDto loginDto);
    Task<Result<UserIdentityDto>> Create(RegisterDto registerDto);
    Task<Result<UserEntity>> GetOrCreateUser(GoogleIdentity googleIdentity);
    Task<Result<UserEntity>> GetByEmail(string email);
}
