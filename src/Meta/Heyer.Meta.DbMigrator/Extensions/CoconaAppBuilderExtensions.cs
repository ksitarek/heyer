using Cocona.Builder;
using Heyer.Meta.DbMigrator.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Heyer.Meta.DbMigrator.Extensions;

internal static class CoconaAppBuilderExtensions
{
    public static CoconaAppBuilder AddConfiguration(this CoconaAppBuilder builder)
    {
        builder.Configuration.AddConfiguration(
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.api.json")
                .AddJsonFile("appsettings.storage.json")
                .Build());
        return builder;
    }

    public static CoconaAppBuilder AddDependencies(this CoconaAppBuilder builder)
    {
        builder.Services.AddSingleton<IHiringDbConnectionStringProvider, HiringDbConnectionStringProvider>();
        builder.Services.AddSingleton<IJobBoardDbConnectionStringProvider, JobBoardDbConnectionStringProvider>();
        builder.Services.AddSingleton<IStorageDbConnectionStringProvider, StorageDbConnectionStringProvider>();

        builder.Services.AddSingleton<ICompaniesProvider, CompaniesProvider>();

        builder.Services.AddSingleton<IMigrator, Migrator>();

        return builder;
    }

    public static CoconaAppBuilder AddLogging(this CoconaAppBuilder builder)
    {
        builder.Services.AddSerilog((sp, lc) => lc
                                        .ReadFrom.Configuration(builder.Configuration)
                                        .ReadFrom.Services(sp));

        return builder;
    }

    public static CoconaAppBuilder AddMediatR(this CoconaAppBuilder builder)
    {
        builder.Services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssembly(typeof(CoconaAppBuilderExtensions).Assembly));

        return builder;
    }
}