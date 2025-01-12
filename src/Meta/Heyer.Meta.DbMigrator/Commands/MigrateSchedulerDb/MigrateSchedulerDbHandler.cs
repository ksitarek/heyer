using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateSchedulerDb;

internal class MigrateSchedulerDbHandler : IRequestHandler<MigrateSchedulerDb>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IMigrator _migrator;

    public MigrateSchedulerDbHandler(ILogger logger, IConfiguration configuration, IMigrator migrator)
    {
        _logger = logger.ForContext("SourceContext", nameof(MigrateSchedulerDbHandler));
        _configuration = configuration;
        _migrator = migrator;
    }

    public Task Handle(MigrateSchedulerDb request, CancellationToken cancellationToken)
    {
        _logger.Information("Migration of Scheduler database started.");

        var connectionString = _configuration["Scheduler:Npgsql:ConnectionString"];
        if (connectionString is null)
        {
            _logger.Warning("Connection string for Scheduler database is not found in configuration.");
            return Task.CompletedTask;
        }

        _migrator.Migrate("SchedulerDb", connectionString);

        _logger.Information("Scheduler database migration completed successfully.");

        return Task.CompletedTask;
    }
}