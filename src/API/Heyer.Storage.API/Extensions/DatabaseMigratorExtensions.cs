using Heyer.Storage.API.Providers.Registry.Npgsql;

namespace Heyer.Storage.API.Extensions;

public static class DatabaseMigratorExtensions
{
    public static WebApplication EnsureDatabaseIsCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<StorageDbContext>();

        if (context == null)
        {
            // nothing to do
            return app;
        }

        context.Database.EnsureCreated();

        return app;
    }
}