namespace Bll.Auth.Dto;

public enum LoginType
{
    ByEmail,
    ByUsername
}

public record LoginDto(LoginType LoginType, string UsernameOrEmail, string Password);