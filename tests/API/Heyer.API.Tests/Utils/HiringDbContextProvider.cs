using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
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