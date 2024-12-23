using Heyer.BuildingBlocks.Tests;
using Heyer.Storage.API.Client;
using Heyer.Storage.API.Tests.Utils.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Heyer.Storage.API.Tests.IntegrationTests;

internal class ApplicationFactory : AbstractApplicationFactory<Program, IStorageApiClient>
{
    private ApplicationFactory(Dictionary<string, string?> configOverrides)
        : base(configOverrides)
    {
    }

    public static IApplicationFactory<IStorageApiClient> Create() =>
        new ApplicationFactory(new Dictionary<string, string?>
        {
            [Config.RegistryStrategy_Type] = "MongoDB",
            [Config.StorageStrategy_Type] = "Filesystem",
            [Config.StorageStrategy_FilesystemStorage_RootPath] =
                "IntegrationTests/Endpoints/StoreEndpointTests"
        });

    public override IStorageApiClient CreateApiClient()
    {
        var httpClient = CreateClient();
        var apiClient = StorageApiClientFactory.Create(httpClient);

        return apiClient;
    }

    public override IStorageApiClient CreateAuthorizedApiClient(Guid companyId, params string[] permissions)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GenerateJwtToken(companyId, permissions));

        return StorageApiClientFactory.Create(client);
    }

    public override ValueTask DisposeAsync()
    {
        // cleanup test files
        var storePath = GetConfigValue(Config.StorageStrategy_FilesystemStorage_RootPath)?.ToString();
        if (!string.IsNullOrEmpty(storePath) && Directory.Exists(storePath))
        {
            Directory.Delete(storePath, true);
        }

        return base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => { services.AddValidators(); });

        base.ConfigureWebHost(builder);
    }

    private string GenerateJwtToken(Guid companyId, string[] permissions)
    {
        var issuer = GetConfigValue(Config.Jwt_ValidIssuer)!.ToString()!;
        var audience = GetConfigValue(Config.Jwt_ValidAudience)!.ToString()!;
        var secret = GetConfigValue(Config.Jwt_Secret)!.ToString()!;

        return base.GenerateJwtToken(issuer, audience, secret, companyId, permissions);
    }
}