using System.Reflection;
using DbUp;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateStorageDb;

internal class MigrateStorageDbCommandHandler : IRequestHandler<MigrateStorageDb>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public MigrateStorageDbCommandHandler(ILogger logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger.ForContext("SourceContext", nameof(MigrateStorageDbCommandHandler));
    }

    public Task Handle(MigrateStorageDb request, CancellationToken cancellationToken)
    {
        _logger.Information("Migration of Storage database started.");

        var connectionString = _configuration["RegistryStrategy:SqlServerRegistry:ConnectionString"];
        if (connectionString is null)
        {
            _logger.Warning("Connection string for Storage database is not found in configuration.");
            return Task.CompletedTask;
        }

        EnsureDatabase.For.SqlDatabase(connectionString);

        var result = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), opts => opts.Contains("StorageContext"))
            .LogToAutodetectedLog()
            .Build()
            .PerformUpgrade();

        if (!result.Successful)
        {
            _logger.Error("Storage database migration failed: {Error}", result.Error);
            return Task.CompletedTask;
        }

        _logger.Information("Storage database migration completed successfully.");

        return Task.CompletedTask;
    }
}