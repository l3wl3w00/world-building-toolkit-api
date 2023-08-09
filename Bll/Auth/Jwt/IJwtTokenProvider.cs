using System.IdentityModel.Tokens.Jwt;

namespace Bll.Auth.Jwt;

public interface IJwtTokenProvider
{ 
    string Generate(Dictionary<string, string> claims);
}