using DbUp;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateSchedulerDb;

internal class MigrateSchedulerDbHandler : IRequestHandler<MigrateSchedulerDb>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public MigrateSchedulerDbHandler(ILogger logger, IConfiguration configuration)
    {
        _logger = logger.ForContext("SourceContext", nameof(MigrateSchedulerDbHandler));
        _configuration = configuration;
    }

    public Task Handle(MigrateSchedulerDb request, CancellationToken cancellationToken)
    {
        _logger.Information("Migration of Scheduler database started.");

        var connectionString = _configuration["Scheduler:SqlServer:ConnectionString"];
        if (connectionString is null)
        {
            _logger.Warning("Connection string for Scheduler database is not found in configuration.");
            return Task.CompletedTask;
        }

        EnsureDatabase.For.SqlDatabase(connectionString);

        _logger.Information("Scheduler database migration completed successfully.");

        return Task.CompletedTask;
    }
}