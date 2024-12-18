using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heyer.API.Tests.IntegrationTests;

public class ApplicationFactory : WebApplicationFactory<Program>, IApplicationFactory
{
    public static readonly Dictionary<string, string?> InMemoryConfiguration = new()
    {
    };

    private readonly Dictionary<string, string?> _instanceConfigOverrides = new();
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(InMemoryConfiguration);
            config.AddInMemoryCollection(_instanceConfigOverrides);
        });

        return base.CreateHost(builder);
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices((services) =>
        {
        });
        
        base.ConfigureWebHost(builder);
    }
    
    public object? GetConfigValue(string key)
    {
        return Services.GetRequiredService<IConfiguration>()[key];
    }
}