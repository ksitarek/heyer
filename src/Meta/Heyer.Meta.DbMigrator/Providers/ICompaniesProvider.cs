namespace Heyer.Meta.DbMigrator.Providers;

internal interface ICompaniesProvider
{
    IEnumerable<string> GetCompanies();
}