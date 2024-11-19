using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ChatHub.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChatHub.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using ChatHub.Domain.Entity.setup;
using ChatHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;


namespace ChatHub.Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("sqlConnection");

        // Register IUser and User class (or any class implementing IUser)
 

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString)
                   .EnableSensitiveDataLogging();
        });

        services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddSingleton(TimeProvider.System);
        services.AddScoped<ITokenService, TokenService>();


        var jwtSettings = new JWTSettings();
        configuration.GetSection("JWTSettings").Bind(jwtSettings);

        // Log the Key value for debugging (remove in production)
        Console.WriteLine($"JWT Key from configuration: {jwtSettings.Key}");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWTSettings:Issuer"],
                ValidAudience = configuration["JWTSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Check if the request is a SignalR request
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken; // Set the token manually for SignalR connection
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddSignalR();
        services.AddAuthorization();





        return services;
    }
}
