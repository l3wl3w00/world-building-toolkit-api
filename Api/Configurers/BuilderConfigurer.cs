using System.Net;
using Bll.Auth;
using Bll.Exception;
using Bll.Mapper;
using Bll.World;
using Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dal.Entities;
using Hellang.Middleware.ProblemDetails;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

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
            var configurer = new ProblemDetailsConfigurer(options);
            configurer.CreateMapping<EntityNotFoundException>(StatusCodes.Status404NotFound);
            configurer.CreateMapping<EntityAlreadyExistsException>(StatusCodes.Status409Conflict);
        });
        _services.AddAutoMapper(typeof(WorldBuilderProfile));
        _services.AddTransient<IWorldService, WorldService>();
        _services.AddTransient<IUserService, UserService>();
        _services.AddEndpointsApiExplorer();
        _services.AddSwaggerGen();

    }

    private class ProblemDetailsConfigurer
    {
        private readonly ProblemDetailsOptions _options;

        public ProblemDetailsConfigurer(ProblemDetailsOptions options)
        {
            _options = options;
            options.IncludeExceptionDetails = (ctx, ex) => false;

        }
        public void CreateMapping<TException>(int statusCode) where TException : Exception
        {
            _options.Map<TException>((ctx, ex) =>
            {
                var pd = StatusCodeProblemDetails.Create(statusCode);
                pd.Title = ex.Message;
                return pd;
            });
        }
    }


}