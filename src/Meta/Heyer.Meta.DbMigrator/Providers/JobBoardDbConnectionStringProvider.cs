using Microsoft.Extensions.Configuration;

namespace Heyer.Meta.DbMigrator.Providers;

internal class JobBoardDbConnectionStringProvider : IJobBoardDbConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public JobBoardDbConnectionStringProvider(IConfiguration configuration) => _configuration = configuration;

    public string? GetConnectionString() => _configuration["Npgsql:ConnectionString"];
}