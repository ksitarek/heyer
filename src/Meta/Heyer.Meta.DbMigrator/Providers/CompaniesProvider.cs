using Microsoft.Extensions.Configuration;

namespace Heyer.Meta.DbMigrator.Providers;

internal class CompaniesProvider : ICompaniesProvider
{
    private readonly IConfiguration _configuration;

    public CompaniesProvider(IConfiguration configuration) => _configuration = configuration;

    public IEnumerable<string> GetCompanies()
    {
        var companies = _configuration.GetSection("Companies")
            .GetChildren()
            .ToList();

        return companies.Select(x => x.Key);
    }
}