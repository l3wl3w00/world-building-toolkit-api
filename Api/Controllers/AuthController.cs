using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bll.Auth;
using Microsoft.AspNetCore.Mvc;
using Bll.Auth.Dto;
using Bll.Auth.Service;
using Bll.Auth.Settings;
using Bll.Common;
using Bll.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
public class AuthController(IUserService userService, IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserIdentityDto>> Register([FromBody] RegisterDto registerDto)
    {
        return await userService.Create(registerDto);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserIdentityDto>> Login([FromBody] LoginDto loginDto)
    {
        return await authService.Login(loginDto);
    }
    
    [HttpGet(Constants.GoogleRedirectUri)]
    public async Task<ActionResult<string>> OnLoggedInGoogle([FromQuery] string code)
    {
        return await authService.OnLoggedInGoogle(code);
    }
}