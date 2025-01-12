using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure.Configuration;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddJobBoardContext(this IServiceCollection services,
                                                          string connectionString)
    {
        services.AddDbContext<JobBoardContext>(o => o.UseNpgsql(connectionString)
                                                   .EnableServiceProviderCaching(false));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<JobBoardContext>());

        services.AddMongoDbInboxStore<JobBoardContext>();
        services.AddMongoDbOutboxStore<JobBoardContext>();

        return services;
    }

    internal static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IPublishedJobOffersRepository, PublishedJobOffersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}