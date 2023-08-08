using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bll.Auth.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bll.Auth.Jwt;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly TimeProvider _timeProvider;
    private readonly JwtGenerationSettings _jwtSettings;

    public JwtTokenProvider(TimeProvider timeProvider, IOptions<JwtGenerationSettings> jwtOptions)
    {
        _timeProvider = timeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public string Generate(Dictionary<string, string> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                claims
                    .Select(c => new Claim(c.Key, c.Value))
                    .ToList()
            ),
            Expires = _timeProvider.GetUtcNow().DateTime.AddDays(7),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}