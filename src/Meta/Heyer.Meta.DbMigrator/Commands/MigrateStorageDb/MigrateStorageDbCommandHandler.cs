using Heyer.Meta.DbMigrator.Providers;
using MediatR;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateStorageDb;

internal class MigrateStorageDbCommandHandler : IRequestHandler<MigrateStorageDb>
{
    private readonly IStorageDbConnectionStringProvider _connectionStringProvider;
    private readonly ILogger _logger;
    private readonly IMigrator _migrator;

    public MigrateStorageDbCommandHandler(ILogger logger,
                                          IStorageDbConnectionStringProvider connectionStringProvider,
                                          IMigrator migrator)
    {
        _connectionStringProvider = connectionStringProvider;
        _migrator = migrator;
        _logger = logger.ForContext("SourceContext", nameof(MigrateStorageDbCommandHandler));
    }

    public Task Handle(MigrateStorageDb request, CancellationToken cancellationToken)
    {
        _logger.Information("Migration of Storage database started.");

        var connectionString = _connectionStringProvider.GetConnectionString();
        if (connectionString is null)
        {
            _logger.Warning("Connection string for Storage database is not found in configuration.");
            return Task.CompletedTask;
        }

        var result = _migrator.Migrate("StorageContext", connectionString);

        if (!result.Successful)
        {
            _logger.Error("Storage database migration failed: {Error}", result.Error);
            return Task.CompletedTask;
        }

        _logger.Information("Storage database migration completed successfully.");

        return Task.CompletedTask;
    }
}