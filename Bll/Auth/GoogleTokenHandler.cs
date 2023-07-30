using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bll.Auth;

public class GoogleTokenHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public GoogleTokenHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            // Authorization header not in request
            return AuthenticateResult.Fail("Authorization header missing.");
        }

        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var idToken = authHeader.Parameter;

        if (string.IsNullOrWhiteSpace(idToken))
        {
            // No token was provided
            return AuthenticateResult.Fail("Invalid token.");
        }

        try
        {
            // Validate the token
            var validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            // Create the user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, validPayload.Subject),
                new Claim(ClaimTypes.Name, validPayload.Name),
                new Claim(ClaimTypes.Email, validPayload.Email),
                // Add more claims as needed...
            };

            // Create the user identity and claims
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Return success
            return AuthenticateResult.Success(ticket);
        }
        catch (InvalidJwtException)
        {
            // The token was invalid
            return AuthenticateResult.Fail("Invalid token.");
        }

    }
}