using System.Text;
using Chat.Framework.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Chat.Application.Shared;

public abstract class AStartup
{
    private IConfiguration Configuration { get; set; }
    private IHostEnvironment HostEnvironment { get; set; }

    protected AStartup(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        Configuration = configuration;
        HostEnvironment = hostEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
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
                ValidIssuer = Configuration["TokenConfig:Issuer"],
                ValidAudience = Configuration["TokenConfig:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF32.GetBytes(Configuration["TokenConfig:SecretKey"]))
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

        services.AddSignalR();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

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

        services.AddAllAssemblies("Chat");
        services.AddAttributeRegisteredServices();
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        RegisterServices(services);
    }

    public void Configure(IApplicationBuilder app)
    {
        if (HostEnvironment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("CorsPolicy");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        UseMiddlewaresBeforeController(app);
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            EndpointRouteBuilder(endpoints);
        });
    }

    public virtual void RegisterServices(IServiceCollection services)
    {

    }

    public virtual void EndpointRouteBuilder(IEndpointRouteBuilder endpointRouteBuilder)
    {

    }

    public virtual void UseMiddlewaresBeforeController(IApplicationBuilder app)
    {

    }
}