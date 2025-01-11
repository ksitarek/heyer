namespace Heyer.Meta.DbMigrator.Providers;

internal interface IJobBoardDbConnectionStringProvider
{
    string? GetConnectionString();
}