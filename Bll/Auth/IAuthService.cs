using System.IdentityModel.Tokens.Jwt;
using Bll.Auth.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Bll.Auth;

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

    Task<GoogleLoginResultDto> OnLoggedInGoogle(string code);
}