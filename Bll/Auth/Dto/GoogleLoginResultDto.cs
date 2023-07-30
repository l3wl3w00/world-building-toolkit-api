namespace Bll.Auth.Dto;

public record GoogleLoginResultDto(bool Successful, string Token, string Email);