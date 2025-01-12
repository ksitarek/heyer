using DbUp.Engine;

namespace Heyer.Meta.DbMigrator;

public interface IMigrator
{
    DatabaseUpgradeResult Migrate(string name, string connectionString);
    DatabaseUpgradeResult Migrate(string name, string schema, string connectionString);
}