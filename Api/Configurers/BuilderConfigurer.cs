using Bll.Auth;
using Bll.Exception;
using Bll.Mapper;
using Bll.World;
using Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dal.Entities;
using Hellang.Middleware.ProblemDetails;

namespace Api.Configurers;
internal class BuilderConfigurer
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _config;

    internal BuilderConfigurer(IServiceCollection services, IConfiguration config)
    {
        this._services = services;
        this._config = config;
    }

    internal static void Configure(WebApplicationBuilder builder) => 
        new BuilderConfigurer(builder.Services, builder.Configuration).Configure();

    private void Configure()
    {
        _services.AddControllers();
        _services.AddDbContext<WorldBuilderDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("WorldBuilderDb")));
        
        _services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<WorldBuilderDbContext>()
            .AddDefaultTokenProviders();
        _services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (ctx, ex) => false;
            // options.Map<EntityNotFoundException<>>(
            //     (ctx, ex) =>
            //     {
            //         var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
            //         pd.Title = ex.Message;
            //         return pd;
            //     }
            // );
        });
        _services.AddAutoMapper(typeof(WorldBuilderProfile));
        _services.AddTransient<IWorldService, WorldService>();
        _services.AddTransient<IUserService, UserService>();
        _services.AddEndpointsApiExplorer();
        _services.AddSwaggerGen();

    }

}