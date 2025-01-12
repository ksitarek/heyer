using Heyer.Meta.DbMigrator.Providers;
using MediatR;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateJobBoardDb;

internal class MigrateJobBoardDbHandler : IRequestHandler<MigrateJobBoardDb>
{
    private const string Schema = "job_board";
    private readonly IJobBoardDbConnectionStringProvider _connectionStringProvider;
    private readonly ILogger _logger;
    private readonly IMigrator _migrator;

    public MigrateJobBoardDbHandler(ILogger logger,
                                    IJobBoardDbConnectionStringProvider connectionStringProvider,
                                    IMigrator migrator)
    {
        _connectionStringProvider = connectionStringProvider;
        _migrator = migrator;
        _logger = logger.ForContext("SourceContext", nameof(MigrateJobBoardDbHandler));
    }

    public Task Handle(MigrateJobBoardDb request, CancellationToken cancellationToken)
    {
        _logger.Information("Migration of Job Board database started.");

        var connectionString = _connectionStringProvider.GetConnectionString();
        if (connectionString is null)
        {
            _logger.Warning("Connection string for Job Board database is not found in configuration.");
            return Task.CompletedTask;
        }

        var result = _migrator.Migrate("JobBoardContext", Schema, connectionString);

        if (!result.Successful)
        {
            _logger.Error("Job Board database migration failed: {Error}", result.Error);
            return Task.CompletedTask;
        }

        _logger.Information("Job Board database migration completed successfully.");

        return Task.CompletedTask;
    }
}