using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Heyer.API.Tests.Utils;

public static class HiringDbContextProvider
{
    public static HiringDbContext Get(Guid companyId)
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{companyId}:MongoDb:ConnectionString"];
        var databaseName =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{companyId}:MongoDb:DatabaseName"];

        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(databaseName);

        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName)
            .EnableServiceProviderCaching(false)
            .Options;

        return new HiringDbContext(options);
    }
}