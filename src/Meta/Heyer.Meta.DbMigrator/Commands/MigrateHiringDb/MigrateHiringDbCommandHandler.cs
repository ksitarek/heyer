using Heyer.Meta.DbMigrator.Providers;
using MediatR;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.MigrateHiringDb;

internal class MigrateHiringDbCommandHandler : IRequestHandler<MigrateHiringDb>
{
    private readonly ICompaniesProvider _companiesProvider;
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public MigrateHiringDbCommandHandler(ILogger logger, ICompaniesProvider companiesProvider, IMediator mediator)
    {
        _logger = logger.ForContext("SourceContext", nameof(MigrateHiringDbCommandHandler));
        _companiesProvider = companiesProvider;
        _mediator = mediator;
    }

    public Task Handle(MigrateHiringDb request, CancellationToken cancellationToken)
    {
        _logger.Information("Migration of Hiring database started.");

        var companies = _companiesProvider.GetCompanies().ToList();

        if (companies.Count == 0)
        {
            _logger.Warning("No companies found in configuration.");
            return Task.CompletedTask;
        }

        Log.Information("Companies found in configuration: {Companies}", companies);

        var tasks = new List<Task>();

        foreach (var companyId in companies)
        {
            var migrateCompanyHiringDb = new MigrateCompanyHiringDb.MigrateCompanyHiringDb(companyId);
            tasks.Add(_mediator.Send(migrateCompanyHiringDb, cancellationToken));
        }

        return Task.WhenAll(tasks);
    }
}