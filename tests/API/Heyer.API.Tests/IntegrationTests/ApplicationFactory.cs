using Heyer.API.Client;
using Heyer.BuildingBlocks.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Heyer.API.Tests.IntegrationTests;

public class ApplicationFactory : AbstractApplicationFactory<Program, IApiClient>
{
    protected ApplicationFactory(Dictionary<string, string?> configOverrides) : base(configOverrides)
    {
    }

    public static IApplicationFactory<IApiClient> Create() => new ApplicationFactory(new Dictionary<string, string?>());

    public override IApiClient CreateApiClient() => ApiClientFactory.Create(CreateClient());

    public override IApiClient CreateAuthorizedApiClient(Guid companyId, params string[] permissions)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GenerateJwtToken(companyId, permissions));

        return ApiClientFactory.Create(client);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => { });

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