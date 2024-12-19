using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Heyer.Storage.API.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
                                                          IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = GetTokenValidationParameters(configuration);
            });

        services.AddAuthorization();

        return services;
    }

    private static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration["Secret"]))
        {
            throw new ArgumentException("Secret is required for JWT authentication.");
        }

        return new TokenValidationParameters
        {
            ValidateIssuer = bool.Parse(configuration["ValidateIssuer"] ?? "true"),
            ValidateAudience = bool.Parse(configuration["ValidateAudience"] ?? "true"),
            ValidateLifetime = bool.Parse(configuration["ValidateLifetime"] ?? "true"),
            ValidateIssuerSigningKey = bool.Parse(configuration["ValidateIssuerSigningKey"] ?? "true"),
            ValidIssuer = configuration["ValidIssuer"],
            ValidAudience = configuration["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secret"]!))
        };
    }
}