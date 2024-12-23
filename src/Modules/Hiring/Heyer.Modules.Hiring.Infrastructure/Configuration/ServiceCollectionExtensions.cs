using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
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
        IConfiguration companiesConfiguration)
    {
        services.AddScoped<HiringDbContext>(sp =>
        {
            var userDataProvider = sp.GetRequiredService<IUserDataProvider>();
            var companyId = userDataProvider.CompanyId;

            var connectionString = companiesConfiguration[$"{companyId}:MongoDb:ConnectionString"];
            var databaseName = companiesConfiguration[$"{companyId}:MongoDb:DatabaseName"];

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);

            var options = new DbContextOptionsBuilder<HiringDbContext>()
                .UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName)
                .Options;

            return new HiringDbContext(options);
        });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<HiringDbContext>());

        return services;
    }

    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IJobOffersRepository, JobOffersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}