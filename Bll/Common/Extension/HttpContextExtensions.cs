using Bll.Auth.Exception;
using Microsoft.AspNetCore.Http;

namespace Bll.Common.Extension;

public static class HttpContextExtensions
{
    public static Dal.Entities.User GetUserEntity(this HttpContext httpContext)
    {
        return httpContext.Items[Constants.UserKey] as Dal.Entities.User ?? throw new UserNotLoggedInException();
    }
}