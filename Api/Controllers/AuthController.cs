using Microsoft.AspNetCore.Mvc;
using Bll.Auth.Dto;
using Bll.Auth.Service;
using Bll.Common;
using Bll.Common.Result_;
using Bll.User_;

namespace Api.Controllers;

[ApiController]
public class AuthController(IUserService userService, IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserIdentityDto>> Register([FromBody] RegisterDto registerDto)
    {
        var result = await userService.Create(registerDto);
        return result.ThrowIfError();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserIdentityDtoWithToken>> Login([FromBody] LoginDto loginDto)
    {
        var result = await authService.Login(loginDto);
        return result.ThrowIfError();
    }
    
    [HttpGet(Constants.GoogleRedirectUri)]
    public async Task<ActionResult<string>> OnLoggedInGoogle([FromQuery] string code)
    {
        return await authService.OnLoggedInGoogle(code);
    }
}