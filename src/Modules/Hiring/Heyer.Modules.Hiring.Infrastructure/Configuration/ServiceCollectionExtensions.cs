using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Heyer.Modules.Hiring.Infrastructure.Configuration;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddHiringDbContext(
        this IServiceCollection services,
        IConfiguration companiesConfiguration,
        IConfiguration inboxOutboxConfiguration)
    {
        AddPerTenantContext(services, companiesConfiguration);

        services.AddMongoDbInboxStore();
        services.AddMongoDbOutboxStore();

        return services;
    }

    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IJobOffersRepository, JobOffersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static void AddPerTenantContext(IServiceCollection services, IConfiguration companiesConfiguration)
    {
        services.AddScoped<IMongoClient>(sp =>
        {
            var userDataProvider = sp.GetRequiredService<IUserDataProvider>();
            var companyId = userDataProvider.CompanyId;

            var connectionString = companiesConfiguration[$"{companyId}:MongoDb:ConnectionString"];

            return new MongoClient(connectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            var userDataProvider = sp.GetRequiredService<IUserDataProvider>();

            var companyId = userDataProvider.CompanyId;

            var databaseName = companiesConfiguration[$"{companyId}:MongoDb:DatabaseName"];

            return sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName);
        });

        services.AddScoped<HiringDbContext>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();

            var options = new DbContextOptionsBuilder<HiringDbContext>()
                .UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName)
                .EnableServiceProviderCaching(false)
                .Options;

            return new HiringDbContext(options);
        });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<HiringDbContext>());
    }
}