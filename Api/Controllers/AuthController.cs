using Bll.Auth;
using Microsoft.AspNetCore.Mvc;
using Bll.Auth.Dto;

namespace Api.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserIdentityDto>> Register([FromBody] RegisterDto registerDto)
    {
        return await _userService.Register(registerDto);
    }
}