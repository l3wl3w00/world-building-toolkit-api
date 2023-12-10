namespace Bll.Auth.Dto;

public record UserIdentityDtoWithToken(string Username, string Email, string Token);
public record UserIdentityDto(string Username, string Email);