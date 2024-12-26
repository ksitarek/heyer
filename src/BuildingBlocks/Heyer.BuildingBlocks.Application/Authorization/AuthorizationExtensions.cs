using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Heyer.BuildingBlocks.Application.Authorization;

public static class AuthorizationExtensions
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services,
                                                         IConfiguration jwtConfiguration)
    {
        services.AddUserDataProvider();
        services.AddSingleton<IUserPermissionChecker, ClaimsPermissionChecker>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = GetTokenValidationParameters(jwtConfiguration);
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("HasPermission",
                              policy =>
                              {
                                  // policy.Requirements.Add(new HasPermissionAuthorizationRequirement());
                                  policy.RequireAuthenticatedUser();
                                  policy.AddAuthenticationSchemes("Bearer");
                              });
        });

        services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
    }

    public static IServiceCollection AddUserDataProvider(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ValueUserDataProvider>();
        services.AddTransient<IUserDataProvider>(sp =>
        {
            var httpContextAccessor = sp.GetService<IHttpContextAccessor>();
            if (httpContextAccessor?.HttpContext != null &&
                httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var userDataProvider = new ClaimsUserDataProvider(httpContextAccessor);
                if (userDataProvider.CompanyId != Guid.Empty)
                {
                    return userDataProvider;
                }
            }

            var vudp = sp.GetRequiredService<ValueUserDataProvider>();
            return vudp;
        });

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