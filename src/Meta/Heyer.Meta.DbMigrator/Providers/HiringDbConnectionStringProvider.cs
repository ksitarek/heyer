using Microsoft.Extensions.Configuration;

namespace Heyer.Meta.DbMigrator.Providers;

internal class HiringDbConnectionStringProvider : IHiringDbConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public HiringDbConnectionStringProvider(IConfiguration configuration) => _configuration = configuration;

    public string? GetConnectionString(string companyId) =>
        _configuration[$"Companies:{companyId}:Npgsql:ConnectionString"];
}