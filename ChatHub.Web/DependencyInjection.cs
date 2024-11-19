using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ChatHub.Application.Common.Interfaces;
using ChatHub.Infrastructure;
using ChatHub.Web.Services;


namespace ChatHub.Web;

public static class DependencyInjection
{
    
    
    private static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "API",
                License = new OpenApiLicense()
                {

                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
        });
    }

    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {

        services.AddScoped<IUser, CurrentUser>();





        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();
        services.RegisterSwagger();
        return services;
    }


}

