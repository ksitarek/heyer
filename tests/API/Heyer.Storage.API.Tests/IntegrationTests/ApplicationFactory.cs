using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Heyer.Storage.API.Tests.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Heyer.Storage.API.Tests.IntegrationTests;

internal class ApplicationFactory : WebApplicationFactory<Program>
{
    public static readonly Dictionary<string, string?> InMemoryConfiguration = new()
    {
        [Config.RegistryStrategy_MongoDbRegistry_ConnectionString] = "",
    };

    private readonly Dictionary<string, string?> _instanceConfigOverrides = new();

    public ApplicationFactory()
    {
    }

    public ApplicationFactory(Dictionary<string, string?> configOverrides)
    {
        _instanceConfigOverrides = configOverrides;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(InMemoryConfiguration);
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

    public override ValueTask DisposeAsync()
    {
        // cleanup test files
        var storePath = GetConfigValue(Config.StorageStrategy_FilesystemStorage_RootPath)?.ToString();
        if (!string.IsNullOrEmpty(storePath) && Directory.Exists(storePath))
            Directory.Delete(storePath, true);

        return base.DisposeAsync();
    }

    public static ApplicationFactory Create()
    {
        return new(new()
        {
            [Config.RegistryStrategy_Type] = "MongoDB",
            [Config.StorageStrategy_Type] = "Filesystem",
            [Config.StorageStrategy_FilesystemStorage_RootPath] = "IntegrationTests/Endpoints/StoreEndpointTests",
        });
    }

    public HttpClient CreateAuthorizedClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GenerateJwtToken());

        return client;
    }
    
    private string GenerateJwtToken()
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetConfigValue(Config.Jwt_Secret)!.ToString()!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: GetConfigValue(Config.Jwt_ValidIssuer)!.ToString(),
            audience: GetConfigValue(Config.Jwt_ValidAudience)!.ToString(),
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}