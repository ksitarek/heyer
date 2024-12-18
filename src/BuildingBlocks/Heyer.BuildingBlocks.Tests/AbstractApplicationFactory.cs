using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Heyer.BuildingBlocks.Tests;

public abstract class AbstractApplicationFactory<TProgram, TApiClient> : 
    WebApplicationFactory<TProgram>, IApplicationFactory<TApiClient>
    where TProgram : class
{
    private readonly Dictionary<string, string?> _instanceConfigOverrides = new();

    protected AbstractApplicationFactory(Dictionary<string, string?> configOverrides)
    {
        _instanceConfigOverrides = configOverrides;
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(ApplicationFactoryConfiguration.InMemoryConfiguration);
            config.AddInMemoryCollection(_instanceConfigOverrides);
        });

        return base.CreateHost(builder);
    }

    public object? GetConfigValue(string key)
    {
        return Services.GetRequiredService<IConfiguration>()[key];
    }

    public TService GetRequiredService<TService>() where TService : class
    {
        var svc = Services.GetRequiredService(typeof(TService));

        return svc as TService ?? throw new InvalidOperationException($"Service of type {typeof(TService)} not found.");
    }
    
    public abstract TApiClient CreateApiClient();
    public abstract TApiClient CreateAuthorizedApiClient();
    
    private string GenerateJwtToken(string jwtIssuer, string jwtAudience, string jwtSecret)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}