using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Heyer.BuildingBlocks.Application.Authorization;

public static class AuthorizationExtensions
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services,
                                                         IConfiguration jwtConfiguration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IUserPermissionChecker, ClaimsPermissionChecker>();
        services.AddScoped<IUserDataProvider, ClaimsUserDataProvider>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = GetTokenValidationParameters(jwtConfiguration);
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("HasPermission", policy =>
            {
                // policy.Requirements.Add(new HasPermissionAuthorizationRequirement());
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes("Bearer");
            });
        });

        services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
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