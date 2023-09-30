using System.IdentityModel.Tokens.Jwt;
using Bll.Auth.Exception;
using Bll.Common;
using Bll.User;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Bll.Auth;

public class UserResolverMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
{
    public async Task Invoke(HttpContext context)
    {
        using var scope = serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token))
        {
            await next(context);
            return;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email") ?? throw new NoClaimInTokenException("email");
        
        var user = await userService.GetByEmail(emailClaim.Value);
        context.Items[Constants.UserKey] = user;
        await next(context);
    }
}