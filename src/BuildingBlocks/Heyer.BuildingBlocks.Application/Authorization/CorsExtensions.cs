using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Application.Authorization;

public static class CorsExtensions
{
    private const string PolicyName = "UiCorsPolicy";

    public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration corsConfiguration)
    {
        var origins = corsConfiguration.GetSection("AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName,
                              builder =>
                              {
                                  builder.WithOrigins(origins!)
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials();
                              });
        });

        return services;
    }

    public static IApplicationBuilder UsePreconfiguredCors(this IApplicationBuilder app) => app.UseCors(PolicyName);
}