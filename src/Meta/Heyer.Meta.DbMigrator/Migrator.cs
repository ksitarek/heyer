using DbUp;
using DbUp.Engine;

namespace Heyer.Meta.DbMigrator;

public class Migrator : IMigrator
{
    public DatabaseUpgradeResult Migrate(string name, string connectionString)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        return DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Migrator).Assembly,
                                           opts => opts.Contains("MigrationFiles") &&
                                                   opts.Contains(name))
            .LogToAutodetectedLog()
            .Build()
            .PerformUpgrade();
    }

    public DatabaseUpgradeResult Migrate(string name, string schema, string connectionString)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        return DeployChanges.To
            .PostgresqlDatabase(connectionString, schema)
            .WithScriptsEmbeddedInAssembly(typeof(Migrator).Assembly,
                                           opts => opts.Contains("MigrationFiles") &&
                                                   opts.Contains(name))
            .LogToAutodetectedLog()
            .Build()
            .PerformUpgrade();
    }
}