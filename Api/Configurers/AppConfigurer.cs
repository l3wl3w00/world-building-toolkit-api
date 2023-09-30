using Bll.Auth;
using Hellang.Middleware.ProblemDetails;

namespace Api.Configurers;
internal class AppConfigurer(WebApplication app)
{
    internal static void Configure(WebApplication app)
    {
        new AppConfigurer(app).Configure();
    }

    private void Configure()
    {
        app.UseProblemDetails();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<UserResolverMiddleware>();

        app.MapControllers();
    }
}