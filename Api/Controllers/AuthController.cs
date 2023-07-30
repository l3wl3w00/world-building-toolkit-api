using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bll.Auth;
using Microsoft.AspNetCore.Mvc;
using Bll.Auth.Dto;
using Bll.Auth.Settings;
using Bll.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserIdentityDto>> Register([FromBody] RegisterDto registerDto)
    {
        return await _userService.Create(registerDto);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserIdentityDto>> Login([FromBody] LoginDto loginDto)
    {
        return await _authService.Login(loginDto);
    }
    
        
    [HttpGet(Constants.GoogleRedirectUri)]

    public async Task<ActionResult<GoogleLoginResultDto>> OnLoggedInGoogle([FromQuery] string code)
    {
        return await _authService.OnLoggedInGoogle(code);
    }
}