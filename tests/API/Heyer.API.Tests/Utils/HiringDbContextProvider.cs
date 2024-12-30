using Heyer.API.Tests.IntegrationTests;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Heyer.API.Tests.Utils;

internal static class HiringDbContextProvider
{
    public static HiringDbContext Get(Guid companyId)
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{companyId}:SqlServer:ConnectionString"];

        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseSqlServer(connectionString)
            .EnableServiceProviderCaching(false)
            .Options;

        return new HiringDbContext(options);
    }
}

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