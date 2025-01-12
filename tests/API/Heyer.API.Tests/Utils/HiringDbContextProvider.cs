using Heyer.BuildingBlocks.Tests;
using Heyer.Meta.DbMigrator;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Heyer.API.Tests.Utils;

internal static class HiringDbContextProvider
{
    private static bool _migrated;

    public static HiringDbContext Get(Guid companyId)
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{companyId}:Npgsql:ConnectionString"];

        EnsureMigrated(connectionString);

        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseNpgsql(connectionString)
            .EnableServiceProviderCaching(false)
            .Options;

        var ctx = new HiringDbContext(options);
        ctx.Database.EnsureCreated();

        return ctx;
    }

    private static void EnsureMigrated(string? connectionString)
    {
        if (!_migrated)
        {
            var migrator = new Migrator();
            migrator.Migrate("HiringContext", connectionString!);
            _migrated = true;
        }
    }
}