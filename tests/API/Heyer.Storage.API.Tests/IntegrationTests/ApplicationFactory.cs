using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Heyer.BuildingBlocks.Tests;
using Heyer.Storage.API.Client;
using Heyer.Storage.API.Tests.Utils.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.IdentityModel.Tokens;

namespace Heyer.Storage.API.Tests.IntegrationTests;

internal class ApplicationFactory : AbstractApplicationFactory<Program, IStorageApiClient>
{
    private ApplicationFactory(Dictionary<string, string?> configOverrides)
        : base(configOverrides)
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices((services) => { services.AddValidators(); });

        base.ConfigureWebHost(builder);
    }

    public override ValueTask DisposeAsync()
    {
        // cleanup test files
        var storePath = GetConfigValue(Config.StorageStrategy_FilesystemStorage_RootPath)?.ToString();
        if (!string.IsNullOrEmpty(storePath) && Directory.Exists(storePath))
            Directory.Delete(storePath, true);

        return base.DisposeAsync();
    }

    public static IApplicationFactory<IStorageApiClient> Create()
    {
        return new ApplicationFactory(new()
        {
            [Config.RegistryStrategy_Type] = "MongoDB",
            [Config.StorageStrategy_Type] = "Filesystem",
            [Config.StorageStrategy_FilesystemStorage_RootPath] = "IntegrationTests/Endpoints/StoreEndpointTests",
        });
    }

    public override IStorageApiClient CreateApiClient()
    {
        var httpClient = CreateClient();
        var apiClient = StorageApiClientFactory.Create(httpClient);

        return apiClient;
    }

    public override IStorageApiClient CreateAuthorizedApiClient(params string[] permissions)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GenerateJwtToken(permissions));

        return StorageApiClientFactory.Create(client);
    }
    
    private string GenerateJwtToken(string[] permissions)
    {
        var issuer = GetConfigValue(Config.Jwt_ValidIssuer)!.ToString()!;
        var audience = GetConfigValue(Config.Jwt_ValidAudience)!.ToString()!;
        var secret = GetConfigValue(Config.Jwt_Secret)!.ToString()!;
        
        return base.GenerateJwtToken(issuer, audience, secret, permissions);
    }
}