using System.Reflection;
using DbUp;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateCompanyHiringDb;

internal record MigrateCompanyHiringDb(string CompanyId) : IRequest;

internal class MigrateCompanyHiringDbHandler : IRequestHandler<MigrateCompanyHiringDb>
{
    private readonly IConfiguration _configuration;
    private ILogger _logger;

    public MigrateCompanyHiringDbHandler(ILogger logger, IConfiguration configuration)
    {
        _logger = logger.ForContext("SourceContext", nameof(MigrateCompanyHiringDbHandler));
        _configuration = configuration;
    }

    public Task Handle(MigrateCompanyHiringDb request, CancellationToken cancellationToken)
    {
        _logger = _logger.ForContext("CompanyId", request.CompanyId);

        _logger.Information("Migration of Company Hiring database started.");

        var connectionString = _configuration[$"Companies:{request.CompanyId}:SqlServer:ConnectionString"];
        if (connectionString is null)
        {
            _logger.Warning("Connection string for Company Hiring database is not found in configuration.");
            return Task.CompletedTask;
        }

        EnsureDatabase.For.SqlDatabase(connectionString);

        var result = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), opts => opts.Contains("HiringContext"))
            .LogToAutodetectedLog()
            .Build()
            .PerformUpgrade();

        if (!result.Successful)
        {
            _logger.Error("Company Hiring database migration failed: {Error}", result.Error);
            return Task.CompletedTask;
        }

        _logger.Information("Company Hiring database migration completed successfully.");

        return Task.CompletedTask;
    }
}