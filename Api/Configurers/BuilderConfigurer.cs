using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bll.Auth;
using Bll.Auth.Exception;
using Bll.Auth.Settings;
using Bll.Common;
using Bll.Common.Exception;
using Bll.Common.Mapper;
using Bll.Continent.Service;
using Bll.User;
using Bll.World;
using Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dal.Entities;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;
using Microsoft.Extensions.Logging;

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
        Database();
        SetAuthentication(); // this has to be after the identity EF config
        SetOptions();
        Controllers();
        ProblemDetails();
        RegisterServices();
        Logging();
        _services.AddEndpointsApiExplorer();
        _services.AddSwaggerGen();

    }

    private void SetAuthentication()
    {
        _services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultScheme = GoogleDefaults.AuthenticationScheme;   
            })
            .AddScheme<AuthenticationSchemeOptions, GoogleTokenHandler>(GoogleDefaults.AuthenticationScheme, null);

    }

    private void SetOptions()
    {
        _config["Authentication:Google:RedirectUri"] = Constants.GoogleRedirectUri;
        _services.Configure<GoogleOAuthSettings>(_config.GetSection("Authentication:Google"));
    }

    private void Logging()
    {
        _services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });
    }

    private void RegisterServices()
    {
        _services.AddAutoMapper(typeof(WorldBuilderProfile));
        _services.AddTransient<IWorldService, WorldService>();
        _services.AddTransient<RegisterErrorExceptionMapper>();
        _services.AddTransient<IUserService, UserService>();
        _services.AddTransient<IAuthService, AuthService>();
        _services.AddTransient<IContinentService, ContinentService>();
    }

    private void ProblemDetails()
    {
        _services.AddProblemDetails(options =>
        {
            var configurer = new ProblemDetailsConfigurer(options);
            configurer.CreateMapping<EntityNotFoundException>(StatusCodes.Status404NotFound);
            configurer.CreateMapping<EntityAlreadyExistsException>(StatusCodes.Status409Conflict);
            configurer.CreateMapping<InvalidPasswordException>(StatusCodes.Status400BadRequest);
            configurer.CreateMapping<NotSupportedLoginTypeException>(StatusCodes.Status400BadRequest);
            configurer.CreateMapping<RegisterException>(StatusCodes.Status500InternalServerError);
            configurer.CreateMapping<LoginException>(StatusCodes.Status401Unauthorized);
            configurer.CreateMapping<UserRegisteredThroughOAuthException>(StatusCodes.Status401Unauthorized);
            configurer.CreateMapping<GoogleJwtGenerationException>(StatusCodes.Status401Unauthorized);
        });
    }

    private void Database()
    {
        _services.AddDbContext<WorldBuilderDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("WorldBuilderDb")));

        _services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<WorldBuilderDbContext>()
            .AddDefaultTokenProviders();
    }

    private void Controllers()
    {
        var converter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);
        _services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(converter)
        );
    }

    private sealed class ProblemDetailsConfigurer
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