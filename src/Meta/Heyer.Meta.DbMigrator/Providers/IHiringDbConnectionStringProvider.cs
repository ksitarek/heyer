namespace Heyer.Meta.DbMigrator.Providers;

internal interface IHiringDbConnectionStringProvider
{
    string? GetConnectionString(string companyId);
}