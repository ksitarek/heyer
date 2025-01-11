namespace Heyer.Meta.DbMigrator.Providers;

internal interface IStorageDbConnectionStringProvider
{
    string? GetConnectionString();
}