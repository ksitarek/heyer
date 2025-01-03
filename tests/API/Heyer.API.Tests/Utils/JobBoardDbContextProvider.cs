using Heyer.API.Tests.IntegrationTests;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Heyer.API.Tests.Utils;

internal static class JobBoardDbContextProvider
{
    public static JobBoardContext Get()
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[Config.SqlServer_ConnectionString];

        var options = new DbContextOptionsBuilder<JobBoardContext>()
            .UseSqlServer(connectionString)
            .EnableServiceProviderCaching(false)
            .Options;

        return new JobBoardContext(options);
    }
}