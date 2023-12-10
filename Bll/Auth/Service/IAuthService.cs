using Bll.Auth.Dto;
using Bll.Common.Result_;

namespace Bll.Auth.Service;

public interface IAuthService
{
    public record GoogleTokenResponseDto(
        string AccessToken,
        int ExpiresIn,
        string RefreshToken,
        string Scope,
        string TokenType,
        string IdToken);

    Task<GoogleTokenResponseDto> CreateTokenForGoogleAsync(string code);
    public Task<Result<UserIdentityDtoWithToken>> Login(LoginDto loginDto);

    Task<string> OnLoggedInGoogle(string code);
}