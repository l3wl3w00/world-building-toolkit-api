﻿using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bll.Auth;
using Bll.Auth.Exception;
using Bll.Auth.Exception.Helper;
using Bll.Auth.Jwt;
using Bll.Auth.Service;
using Bll.Auth.Settings;
using Bll.Common;
using Bll.Common.Exception;
using Bll.Common.Mapper;
using Bll.Continent.Service;
using Bll.User;
using Bll.Planet;
using Bll.Region;
using Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dal.Entities;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;
using Microsoft.IdentityModel.Tokens;

namespace Api.Configurers;
internal class BuilderConfigurer(IServiceCollection services, IConfiguration config)
{
    private readonly JwtGenerationSettings _jwtGenerationSettings = new();
    private readonly IConfigurationSection _jwtSection = config.GetSection("Authentication:Jwt");
    
    internal static void Configure(WebApplicationBuilder builder) => 
        new BuilderConfigurer(builder.Services, builder.Configuration).Configure();

    private void Configure()
    {
        _jwtSection.Bind(_jwtGenerationSettings);
        Database();
        SetAuthentication(); // this has to be after the identity EF config
        SetOptions();
        Controllers();
        ProblemDetails();
        RegisterServices();
        Logging();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

    }

    private void SetAuthentication()
    {
        services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, GoogleTokenHandler>(GoogleDefaults.AuthenticationScheme, null)
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.TokenValidationParameters = new()
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtGenerationSettings.Secret)),

                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = _jwtGenerationSettings.Issuer,

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = _jwtGenerationSettings.Audience,

                    // Validate the token expiry
                    ValidateLifetime = true,
                };
            });
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme,
                GoogleDefaults.AuthenticationScheme);
            defaultAuthorizationPolicyBuilder = 
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });
    }

    private void SetOptions()
    {
        config["Authentication:Google:RedirectUri"] = Constants.GoogleRedirectUri;
        services.Configure<GoogleOAuthSettings>(config.GetSection("Authentication:Google"));
        services.Configure<JwtGenerationSettings>(_jwtSection);
    }

    private void Logging()
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });
    }

    private void RegisterServices()
    {
        services.AddAutoMapper(typeof(WorldBuilderProfile));
        services.AddTransient<IPlanetService, PlanetService>();
        services.AddTransient<RegisterErrorExceptionMapper>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IContinentService, ContinentService>();
        services.AddTransient<IRegionService, RegionService>();
        services.AddTransient<IJwtTokenProvider, JwtTokenProvider>();
    }

    private void ProblemDetails()
    {
        services.AddProblemDetails(options =>
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
        services.AddDbContext<WorldBuilderDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("WorldBuilderDb")));

        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<WorldBuilderDbContext>()
            .AddDefaultTokenProviders();
    }

    private void Controllers()
    {
        var converter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);
        var mvcBuilder = services.AddControllers();
        mvcBuilder.AddJsonOptions(options =>
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