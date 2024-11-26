using Heyer.Storage.API.Tests.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heyer.Storage.API.Tests.IntegrationTests;

internal class ApplicationFactory : WebApplicationFactory<Program>
{
    public static readonly Dictionary<string, string?> InMemoryConfiguration = new()
    {
        [Config.RegistryStrategy_MongoDbRegistry_ConnectionString] = "",
    };
    
    public readonly Dictionary<string, string?> InstanceConfigOverrides = new();

    public ApplicationFactory()
    {
        
    }
    
    public ApplicationFactory(Dictionary<string, string?> configOverrides)
    {
        InstanceConfigOverrides = configOverrides;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(InMemoryConfiguration);
            config.AddInMemoryCollection(InstanceConfigOverrides);
        });

        return base.CreateHost(builder);
    }
    
    public object? GetConfigValue(string key)
    {
        return this.Services.GetRequiredService<IConfiguration>()[key];
    }

    public TService GetRequiredService<TService>() where TService : class
    {
        var svc = this.Services.GetRequiredService(typeof(TService));
        
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
}