using Cocona.Builder;
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