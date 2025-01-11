using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateHiringDb;

internal class MigrateHiringDbCommandHandler : IRequestHandler<MigrateHiringDb>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public MigrateHiringDbCommandHandler(ILogger logger, IConfiguration configuration, IMediator mediator)
    {
        _logger = logger.ForContext("SourceContext", nameof(MigrateHiringDbCommandHandler));
        _configuration = configuration;
        _mediator = mediator;
    }

    public Task Handle(MigrateHiringDb request, CancellationToken cancellationToken)
    {
        _logger.Information("Migration of Hiring database started.");

        var companies = _configuration.GetSection("Companies")
            .GetChildren()
            .ToList();

        if (companies.Count == 0)
        {
            _logger.Warning("No companies found in configuration.");
            return Task.CompletedTask;
        }

        Log.Information("Companies found in configuration: {Companies}", companies.Select(x => x.Key));

        var tasks = new List<Task>();

        foreach (var company in companies)
        {
            var companyId = company.Key;

            var migrateCompanyHiringDb = new MigrateCompanyHiringDb.MigrateCompanyHiringDb(companyId);
            tasks.Add(_mediator.Send(migrateCompanyHiringDb, cancellationToken));
        }

        return Task.WhenAll(tasks);
    }
}