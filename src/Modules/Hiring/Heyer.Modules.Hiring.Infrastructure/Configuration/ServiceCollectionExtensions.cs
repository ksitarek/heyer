using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure.Configuration;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddHiringDbContext(
        this IServiceCollection services,
        IConfiguration companiesConfiguration)
    {
        AddPerClientContext(services, companiesConfiguration);

        services.AddMongoDbInboxStore<HiringDbContext>();
        services.AddMongoDbOutboxStore<HiringDbContext>();

        return services;
    }

    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IJobOffersRepository, JobOffersRepository>();
        services.AddScoped<ICandidatesRepository, CandidatesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static void AddPerClientContext(IServiceCollection services, IConfiguration companiesConfiguration)
    {
        services.AddScoped<HiringDbContext>(sp =>
        {
            var userDataProvider = sp.GetRequiredService<IUserDataProvider>();
            var companyId = userDataProvider.CompanyId;

            var connectionString = companiesConfiguration[$"{companyId}:Npgsql:ConnectionString"];

            var options = new DbContextOptionsBuilder<HiringDbContext>()
                .UseNpgsql(connectionString)
                .EnableServiceProviderCaching(false)
                .Options;

            return new HiringDbContext(options);
        });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<HiringDbContext>());
    }
}