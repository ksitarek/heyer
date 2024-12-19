using Heyer.API.Client;
using Heyer.API.Tests.Utils;
using Heyer.BuildingBlocks.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.API.Tests.IntegrationTests;

public class ApplicationFactory : AbstractApplicationFactory<Program, IApiClient>
{
    protected ApplicationFactory(Dictionary<string, string?> configOverrides) : base(configOverrides)
    {
    }

    public static IApplicationFactory<IApiClient> Create() => new ApplicationFactory(new Dictionary<string, string?>());

    public override IApiClient CreateApiClient() => ApiClientFactory.Create(CreateClient());

    public override IApiClient CreateAuthorizedApiClient(params string[] permissions)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GenerateJwtToken(permissions));

        return ApiClientFactory.Create(client);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => { services.AddScoped<JobOfferValidator>(); });

        base.ConfigureWebHost(builder);
    }

    private string GenerateJwtToken(string[] permissions)
    {
        var issuer = GetConfigValue(Config.Jwt_ValidIssuer)!.ToString()!;
        var audience = GetConfigValue(Config.Jwt_ValidAudience)!.ToString()!;
        var secret = GetConfigValue(Config.Jwt_Secret)!.ToString()!;

        return GenerateJwtToken(issuer, audience, secret, permissions);
    }
}