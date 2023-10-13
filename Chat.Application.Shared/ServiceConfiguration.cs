using Chat.Framework.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Chat.Application.Shared;

public static class ServiceConfiguration
{
    public static IServiceCollection AddCommonServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["TokenConfig:Issuer"],
                ValidAudience = configuration["TokenConfig:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF32.GetBytes(configuration["TokenConfig:SecretKey"]))
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (string.IsNullOrEmpty(path) == false &&
                        string.IsNullOrEmpty(accessToken) == false)
                    {
                        context.HttpContext.Request.Headers.Authorization = accessToken;
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(host => true)
                .AllowCredentials();
        }));

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpClient();
        services.AddHttpContextAccessor();

        services.AddAllAssemblies("Chat");
        services.AddAttributeRegisteredServices();

        return services;
    }
}