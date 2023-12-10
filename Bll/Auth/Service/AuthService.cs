﻿using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Bll.Auth.Dto;
using Bll.Auth.Exception;
using Bll.Auth.Jwt;
using Bll.Auth.Settings;
using Bll.Common.Result_;
using Bll.User_;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Bll.Auth.Service;

public class AuthService(
        IOptions<GoogleOAuthSettings> googleOAuthOptions,
        IUserService userService,
        UserManager<Dal.Entities.User> userManager, 
        IJwtTokenProvider jwtTokenProvider)
    : IAuthService
{
    private readonly GoogleOAuthSettings _googleOAuthValues = googleOAuthOptions.Value;

    public async Task<Result<UserIdentityDtoWithToken>> Login(LoginDto loginDto)
    {
        var findUserResult = await userService.FindUserByLogin(loginDto);
        if (findUserResult.IsError) return findUserResult.Into<UserIdentityDtoWithToken>();
        var user = findUserResult.OkValue;
        var passwordHash = user.PasswordHash ?? throw new UserRegisteredThroughOAuthException();
        var result = userManager.PasswordHasher.VerifyHashedPassword(user,passwordHash, loginDto.Password);
        if (result != PasswordVerificationResult.Success) throw new LoginException();

        if (user.Email == null || user.UserName == null)
        {
            return new System.Exception($"User {user.Id} has no username or email").ToErrorResult<UserIdentityDtoWithToken>();
        }
        var token = jwtTokenProvider.Generate(new Dictionary<string, string>
        {
            { "email", user.Email },
            { "username", user.UserName },
        });
        return new UserIdentityDtoWithToken(user.UserName, user.Email, token).ToOkResult();
    }

    // Should be abstracted if more oauth provider
    public async Task<string> OnLoggedInGoogle(string code)
    {
        var response = await CreateTokenForGoogleAsync(code);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(response.IdToken) as JwtSecurityToken;

        var nameClaim = token!.Claims.First(c => c.Type == "name");
        var emailClaim = token.Claims.First(c => c.Type == "email");

        var userResult = await userService.GetOrCreateUser(new GoogleIdentity(nameClaim.Value, emailClaim.Value));
        userResult.ThrowIfError();
        return "Successfully logged in! You can now navigate back to the app!";
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