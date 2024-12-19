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
    private readonly Dictionary<string, string?> _instanceConfigOverrides;
    private IServiceScope? _testServiceScope;

    protected AbstractApplicationFactory(Dictionary<string, string?> configOverrides) =>
        _instanceConfigOverrides = configOverrides;

    public abstract TApiClient CreateApiClient();
    public abstract TApiClient CreateAuthorizedApiClient(params string[] permissions);

    public override ValueTask DisposeAsync()
    {
        _testServiceScope?.Dispose();

        return base.DisposeAsync();
    }

    public object? GetConfigValue(string key) => Services.GetRequiredService<IConfiguration>()[key];

    public TService GetRequiredService<TService>() where TService : class
    {
        _testServiceScope ??= Services.CreateScope();

        var svc = _testServiceScope.ServiceProvider.GetRequiredService(typeof(TService));

        return svc as TService ?? throw new InvalidOperationException($"Service of type {typeof(TService)} not found.");
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

    protected string GenerateJwtToken(string jwtIssuer, string jwtAudience, string jwtSecret, string[] permissions)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("CompanyId", Guid.Parse("A62C048C-8E0F-41E2-84D4-BD061F9DDE97").ToString()),
            new Claim("CompanyName", "ACME Corporation")
        };

        claims = claims.Concat(permissions.Select(permission => new Claim("permissions", permission))).ToArray();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwtIssuer,
            jwtAudience,
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}