﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using Bll.Auth.Dto;
using Bll.Auth.Exception;
using Bll.Auth.Jwt;
using Bll.Auth.Settings;
using Bll.User;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bll.Auth;

public class AuthService : IAuthService
{
    private readonly GoogleOAuthSettings _googleOAuthValues;
    private readonly IUserService _userService;
    private readonly UserManager<Dal.Entities.User> _userManager;
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public AuthService(IOptions<GoogleOAuthSettings> googleOAuthOptions, IUserService userService, UserManager<Dal.Entities.User> userManager, IJwtTokenProvider jwtTokenProvider)
    {
        _userService = userService;
        _userManager = userManager;
        _jwtTokenProvider = jwtTokenProvider;
        _googleOAuthValues = googleOAuthOptions.Value;
    }

    public async Task<UserIdentityDto> Login(LoginDto loginDto)
    {
        var user = await _userService.FindUserByLogin(loginDto);
        var passwordHash = user.PasswordHash ?? throw new UserRegisteredThroughOAuthException();
        var result = _userManager.PasswordHasher.VerifyHashedPassword(user,passwordHash, loginDto.Password);
        if (result != PasswordVerificationResult.Success) throw new LoginException();

        var token = _jwtTokenProvider.Generate(new Dictionary<string, string>
        {
            { "email", user.Email },
            { "username", user.UserName },
        });
        return new UserIdentityDto(user.UserName, user.Email, token);
    }

    // Should be abstracted if more oauth provider
    public async Task<GoogleLoginResultDto> OnLoggedInGoogle(string code)
    {
        var response = await CreateTokenForGoogleAsync(code);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(response.IdToken) as JwtSecurityToken;

        var nameClaim = token!.Claims.First(c => c.Type == "name");
        var emailClaim = token.Claims.First(c => c.Type == "email");

        var user = await _userService.GetOrCreateUser(new GoogleIdentity(nameClaim.Value, emailClaim.Value));
        return new GoogleLoginResultDto(true, response.IdToken, user.Email);
    }

    public async Task<IAuthService.GoogleTokenResponseDto> CreateTokenForGoogleAsync(string code)
    {
        using var httpClient = new HttpClient();

        var parameters = new Dictionary<string, string>
        {
            {"client_id", _googleOAuthValues.ClientId},
            {"client_secret", _googleOAuthValues.ClientSecret},
            {"code", code},
            {"redirect_uri", $"https://localhost:44366/{_googleOAuthValues.RedirectUri}"},
            {"grant_type", "authorization_code"}
        };

        var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(parameters)); //TODO remove constant string
        if(!response.IsSuccessStatusCode)
        {
            throw new GoogleJwtGenerationException();
        }

        var jsonString = await response.Content.ReadAsStringAsync();

        
        var result = JsonSerializer.Deserialize<IAuthService.GoogleTokenResponseDto>(jsonString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }) ?? throw new NullReferenceException();
        
        var response2 = await httpClient.GetAsync($"http://localhost:8080/?token={result.IdToken}&refresh_token={result.RefreshToken}&expires={result.ExpiresIn}");
        if (!response2.IsSuccessStatusCode)
        {
            throw new GoogleJwtGenerationException();
        }

        return result;
    }

}