using System.Reflection;
using DbUp;
using DbUp.Helpers;
using Heyer.Meta.DbMigrator.Providers;
using MediatR;
using Serilog;

namespace Heyer.Meta.DbMigrator.Commands.LoadSampleData;

internal class LoadSampleDataHandler : IRequestHandler<LoadSampleData>
{
    private readonly ICompaniesProvider _companiesProvider;
    private readonly IHiringDbConnectionStringProvider _hiringDbConnectionStringProvider;
    private readonly IJobBoardDbConnectionStringProvider _jobBoardDbConnectionStringProvider;
    private readonly ILogger _logger;

    private readonly NullJournal _nullJournal = new();

    public LoadSampleDataHandler(ILogger logger,
                                 ICompaniesProvider companiesProvider,
                                 IJobBoardDbConnectionStringProvider jobBoardDbConnectionStringProvider,
                                 IHiringDbConnectionStringProvider hiringDbConnectionStringProvider)
    {
        _logger = logger.ForContext<LoadSampleDataHandler>();
        _companiesProvider = companiesProvider;
        _jobBoardDbConnectionStringProvider = jobBoardDbConnectionStringProvider;
        _hiringDbConnectionStringProvider = hiringDbConnectionStringProvider;
    }

    public Task Handle(LoadSampleData request, CancellationToken cancellationToken)
    {
        _logger.Information("Loading sample data started.");

        _logger.Information("Resources: \n {resources}", Assembly.GetExecutingAssembly().GetManifestResourceNames());

        HandleDataLoad("JobBoardDb", _jobBoardDbConnectionStringProvider.GetConnectionString());

        foreach (var company in _companiesProvider.GetCompanies())
        {
            HandleDataLoad($"HiringDb_{company}".Replace("-", "_"),
                           _hiringDbConnectionStringProvider.GetConnectionString(company));
        }

        return Task.CompletedTask;
    }

    private void HandleDataLoad(string name, string? connectionString)
    {
        if (connectionString is null)
        {
            _logger.Warning("Failed to load sample data to {db} database. Connection string is null.", name);
            return;
        }

        var result = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                                           opts => opts.Contains("TestData") && opts.Contains(name))
            .JournalTo(_nullJournal)
            .LogToConsole()
            .Build()
            .PerformUpgrade();

        if (!result.Successful)
        {
            _logger.Error("Failed to load sample data to {db} database.", name);
        }
        else
        {
            _logger.Information("Sample data loaded to {db} database.", name);
        }
    }
}