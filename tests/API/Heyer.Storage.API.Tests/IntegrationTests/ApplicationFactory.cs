using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Heyer.Storage.API.Tests.IntegrationTests;

internal class ApplicationFactory : WebApplicationFactory<Program>
{
    public static readonly Dictionary<string, string?> InMemoryConfiguration = new()
    {
        ["StorageStrategy:Type"] = "Filesystem",
        ["StorageStrategy:FilesystemStorage:RootPath"] = "IntegrationTests/Endpoints/StoreEndpointTests",
    };
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(InMemoryConfiguration);
        });
        
        return base.CreateHost(builder);
    }
}