using Heyer.BuildingBlocks.Infrastructure;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Heyer.Modules.JobBoard.Infrastructure.Configuration;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddJobBoardContext(this IServiceCollection services,
                                                          string connectionString,
                                                          string databaseName)
    {
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(databaseName);

        services.AddSingleton(db);

        services.AddDbContext<JobBoardContext>(o => o.UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<JobBoardContext>());

        return services;
    }

    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IPublishedJobOffersRepository, PublishedJobOffersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}