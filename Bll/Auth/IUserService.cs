using Bll.Auth.Dto;

namespace Bll.Auth;

public interface IUserService
{
    Task<UserIdentityDto> Login(LoginDto loginDto);
    Task<UserIdentityDto> Register(RegisterDto registerDto);
}