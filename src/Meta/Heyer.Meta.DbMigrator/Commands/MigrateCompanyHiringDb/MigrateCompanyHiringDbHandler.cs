using Heyer.Meta.DbMigrator.Providers;
using MediatR;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateCompanyHiringDb;

internal class MigrateCompanyHiringDbHandler : IRequestHandler<MigrateCompanyHiringDb>
{
    private readonly IHiringDbConnectionStringProvider _connectionStringProvider;
    private readonly IMigrator _migrator;
    private ILogger _logger;

    public MigrateCompanyHiringDbHandler(ILogger logger,
                                         IHiringDbConnectionStringProvider connectionStringProvider,
                                         IMigrator migrator)
    {
        _connectionStringProvider = connectionStringProvider;
        _migrator = migrator;
        _logger = logger.ForContext("SourceContext", nameof(MigrateCompanyHiringDbHandler));
    }

    public Task Handle(MigrateCompanyHiringDb request, CancellationToken cancellationToken)
    {
        _logger = _logger.ForContext("CompanyId", request.CompanyId);

        _logger.Information("Migration of Company Hiring database started.");

        var connectionString = _connectionStringProvider.GetConnectionString(request.CompanyId);
        if (connectionString is null)
        {
            _logger.Warning("Connection string for Company Hiring database is not found in configuration.");
            return Task.CompletedTask;
        }

        var result = _migrator.Migrate("HiringContext", connectionString);

        if (!result.Successful)
        {
            _logger.Error("Company Hiring database migration failed: {Error}", result.Error);
            return Task.CompletedTask;
        }

        _logger.Information("Company Hiring database migration completed successfully.");

        return Task.CompletedTask;
    }
}