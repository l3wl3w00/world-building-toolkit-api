using Bll.Auth.Dto;

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
    public Task<UserIdentityDto> Login(LoginDto loginDto);

    Task<string> OnLoggedInGoogle(string code);
}