using Heyer.API.Tests.IntegrationTests;
using Heyer.BuildingBlocks.Tests;
using Heyer.Meta.DbMigrator;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Heyer.API.Tests.Utils;

internal static class JobBoardDbContextProvider
{
    private static bool _migrated;

    public static JobBoardContext Get()
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[Config.Npgsql_ConnectionString];

        EnsureMigrated(connectionString);

        var options = new DbContextOptionsBuilder<JobBoardContext>()
            .UseNpgsql(connectionString)
            .EnableServiceProviderCaching(false)
            .Options;

        var ctx = new JobBoardContext(options);

        return ctx;
    }

    private static void EnsureMigrated(string? connectionString)
    {
        if (!_migrated)
        {
            var migrator = new Migrator();
            migrator.Migrate("JobBoardContext", "job_board", connectionString!);
            _migrated = true;
        }
    }
}